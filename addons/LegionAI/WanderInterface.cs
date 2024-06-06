using Godot;
using Godot.NativeInterop;
using Legion.Character.Movement;
using Legion.Character.Movement.Steering;

namespace Legion.addons.legionai;

public partial class WanderInterface : EditorNode3DGizmoPlugin
{
    private const int OffsetHandleId = 0;
    private const int LeftRadiusHandleId = 1;
    private const int RightRadiusHandleId = 2;

    public WanderInterface()
    {
        GD.Print("Creating gizmo plugin");
        CreateMaterial("main", Colors.Orange);
        CreateHandleMaterial("handles");
    }

    public override bool _HasGizmo(Node3D forNode3D)
    {
        return forNode3D is Wander;
    }

    public override void _Redraw(EditorNode3DGizmo gizmo)
    {
        gizmo.Clear();

        Wander wander = (Wander)gizmo.GetNode3D();

        Vector3 wanderOffsetPoint = -Vector3.Forward * wander.WanderOffset;
        Vector3 wanderRadiusRightPoint = wanderOffsetPoint + Vector3.Right * wander.WanderRadius;
        Vector3 wanderRadiusLeftPoint = wanderOffsetPoint + Vector3.Left * wander.WanderRadius;
        TorusMesh torusMesh = new();
        float torusThickness = 0.05f;
        torusMesh.InnerRadius = wander.WanderRadius - torusThickness;
        torusMesh.OuterRadius = wander.WanderRadius + torusThickness;

        Vector3[] lines = { Vector3.Zero, wanderOffsetPoint };
        Vector3[] handles = { wanderOffsetPoint, wanderRadiusRightPoint, wanderRadiusLeftPoint };
        int[] ids = { OffsetHandleId, RightRadiusHandleId, LeftRadiusHandleId };

        gizmo.AddMesh(
            torusMesh,
            GetMaterial("main", gizmo),
            Transform3D.Identity.Translated(wanderOffsetPoint)
        );

        gizmo.AddLines(lines, GetMaterial("main", gizmo));

        gizmo.AddHandles(handles, GetMaterial("handles"), ids);
    }

    public override void _SetHandle(
        EditorNode3DGizmo gizmo,
        int handleId,
        bool secondary,
        Camera3D camera,
        Vector2 screenPos
    )
    {
        Wander wander = (Wander)gizmo.GetNode3D();
        // TODO: This Vector3.Up should probably come from the wander component.
        Plane flat = new(Vector3.Up, wander.GlobalPosition);
        Vector3 rayOrigin = camera.ProjectRayOrigin(screenPos);
        Vector3 rayNormal = camera.ProjectRayNormal(screenPos);
        Vector3? intersectsRayNullable = flat.IntersectsRay(rayOrigin, rayNormal);
        if (!intersectsRayNullable.HasValue)
        {
            return;
        }
        Vector3 gizmoPlaneMouseIntersection = intersectsRayNullable.Value;
        if (handleId is OffsetHandleId)
        {
            SetOffsetHandle(wander, gizmoPlaneMouseIntersection);
        }

        if (handleId is RightRadiusHandleId or LeftRadiusHandleId)
        {
            SetRadiusHandle(wander, gizmoPlaneMouseIntersection);
        }

        wander.UpdateGizmos();
    }

    private void SetRadiusHandle(Wander wander, Vector3 gizmoPlaneMouseIntersection)
    {
        Vector3 forward = -Vector3.Forward;
        Plane horizontalPlane = new(forward, wander.GlobalPosition);
        wander.WanderRadius = horizontalPlane.Project(gizmoPlaneMouseIntersection).Length();
    }

    private void SetOffsetHandle(Wander wander, Vector3 gizmoPlaneMouseIntersection)
    {
        Vector3 forward = -Vector3.Forward;
        Vector3 right = forward.Cross(Vector3.Up).Normalized();
        Plane forwardPlane = new(right, wander.GlobalPosition);
        wander.WanderOffset = forwardPlane.Project(gizmoPlaneMouseIntersection).Length();
    }

    public override string _GetGizmoName()
    {
        return "WanderInterface";
    }
}
