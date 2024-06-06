using Godot;
using Godot.NativeInterop;
using Legion.Character.Movement.Steering;

namespace Legion.addons.legionai;

public partial class WanderInterface : EditorNode3DGizmoPlugin
{
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

        var p = new Vector3(1, 0, 1).Normalized() * wander.WanderOffset;
        Vector3[] lines = { Vector3.Zero, p };
        Vector3[] handles = { p };
        int[] ids = { };

        // gizmo.AddMesh(new PlaneMesh(), GetMaterial("main", gizmo), Transform3D.Identity);

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

        Vector3 intersectsRay = intersectsRayNullable.Value;
        Vector3 forward = new Vector3(1, 0, 1).Normalized();
        Plane forwardPlane = new(forward.Cross(Vector3.Up).Normalized(), wander.GlobalPosition);
        wander.WanderOffset = forwardPlane.Project(intersectsRay).Length();
        wander.UpdateGizmos();
    }

    public override string _GetGizmoName()
    {
        return "WanderInterface";
    }
}
