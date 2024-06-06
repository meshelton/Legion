using Godot;
using Godot.Collections;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/kinematics.svg")]
public partial class SteeringController3D : Node3D
{
    [Export]
    public float MaxSpeed = 10;

    [Export]
    public float MaxRotation = Mathf.Pi / 2;

    public new Vector3 Position
    {
        get => _position;
    }

    public float Orientation
    {
        get => _orientation;
    }

    public Vector3 OrientationVector
    {
        get => -Vector3.Forward.Rotated(Vector3.Up, _orientation).Normalized();
    }

    public Vector3 Velocity
    {
        get => _velocity;
    }

    public new float Rotation
    {
        get => _rotation;
    }

    private Vector3 _position;
    private float _orientation;

    private Vector3 _velocity;
    private float _rotation;

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
    public override void _Process(double delta)
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
        _position += _velocity * time;
        _orientation += _rotation * time;

        _velocity += steering.Linear * time;
        _rotation += steering.Angular * time;

        if (_velocity.Length() > maxSpeed)
        {
            _velocity = _velocity.Normalized();
            _velocity *= maxSpeed;
        }

        if (_rotation > maxRotation)
        {
            _rotation = maxRotation;
        }
    }

    private float NewOrientation(float current, Vector3 velocity)
    {
        if (velocity.Length() > 0)
        {
            return Mathf.Atan2(-velocity.X, velocity.Z);
        }

        return current;
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
