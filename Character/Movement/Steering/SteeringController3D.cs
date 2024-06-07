using Godot;
using Godot.Collections;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/kinematics.svg")]
public partial class SteeringController3D : Node3D
{
    // TODO: Add a configuration environment that allows setting defaults for all these values
    [Export]
    public float MaxSpeed = 10;

    [Export]
    public float MaxRotation = Mathf.Pi / 2;

    [Export]
    public bool IncludeDampening = true;
    public Vector3 Velocity { get; private set; }
    public new float Rotation { get; private set; }

    // Might make sense to get rid of these.
    private Vector3 _position;
    private float _orientation;

    public Node3D Character
    {
        get => GetParent<Node3D>();
    }

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            // Code to execute in editor.
            UpdateConfigurationWarnings();
        }

        _position = GetParent<Node3D>().GlobalPosition;
        _orientation = GetParent<Node3D>().Orientation();
    }

    // TODO: Lack of drag force causes various steering behaviors to oscillate forever
    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint())
        {
            return;
        }

        Array<SteeringBehavior3D> movements = this.GetChildrenOfType<SteeringBehavior3D>();

        foreach (SteeringBehavior3D movement in movements)
        {
            SteeringOutput3D steering = movement.GetSteering();
            Update(steering, MaxSpeed, MaxRotation, (float)delta);
        }

        Character.GlobalPosition = _position;
        Character.SetOrientation(_orientation);
    }

    private void Update(SteeringOutput3D steering, float maxSpeed, float maxRotation, float time)
    {
        _position += Velocity * time;
        _orientation += Rotation * time;

        Velocity += steering.Linear * time;
        Rotation += steering.Angular * time;

        if (Velocity.Length() > maxSpeed)
        {
            Velocity = Velocity.Normalized();
            Velocity *= maxSpeed;
        }

        if (Rotation > maxRotation)
        {
            Rotation = maxRotation;
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        Node parent = GetParent();
        if (parent is not Node3D)
        {
            return new[] { "KinematicsController must be attached to a Node3D" };
        }

        return new string[] { };
    }
}
