using Godot;
using Godot.Collections;
using Legion.Character.Movement.Steering;

namespace Legion.Character.Movement.Kinematic;

[Tool]
[GlobalClass]
[Icon("res://Icons/kinematics.svg")]
public partial class KinematicController3D : Node
{
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

        _position = GetParent<Node3D>().Position;
        _orientation = GetParent<Node3D>().Orientation();
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
        {
            return;
        }

        Array<KinematicBehavior3D> movements = this.GetChildrenOfType<KinematicBehavior3D>();

        foreach (KinematicBehavior3D movement in movements)
        {
            KinematicSteeringOutput3D steering = movement.GetSteering();
            Update(steering, (float)delta);
        }

        GetParent<Node3D>().Position = _position;
        if (_rotation != 0)
        {
            GetParent<Node3D>().Rotate(Vector3.Up, _rotation);
        }
    }

    private void Update(SteeringOutput3D steering, float time)
    {
        float halfTSq = 0.5f * time * time;
        _position += _velocity * time + steering.Linear * halfTSq;
        _orientation += _rotation * time + steering.Angular * halfTSq;

        _velocity += steering.Linear * time;
        _rotation += steering.Angular * time;
    }

    private void Update(KinematicSteeringOutput3D steering, float time)
    {
        _velocity = steering.Velocity;
        _rotation = steering.Rotation;
        _position += _velocity * time;
        _orientation += _rotation * time;
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
