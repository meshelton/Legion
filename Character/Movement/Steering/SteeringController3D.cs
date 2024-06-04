using Godot;
using Godot.Collections;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass, Icon("res://Icons/kinematics.svg")]
public partial class SteeringController3D : Node
{
    [Export] public float MaxSpeed = 10;
    [Export] public float MaxRotation = Mathf.Pi / 2;
    
    public Vector3 Position => _position;

    public float Orientation => _orientation;

    public Vector3 Velocity => _velocity;

    public float Rotation => _rotation;    
    
    private Vector3 _position;
    private float _orientation;
    
    
    private Vector3 _velocity;
    private float _rotation;

    public Node3D Character => GetParent<Node3D>();

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
            var steering = movement.GetSteering();
            Update(steering, MaxSpeed, MaxRotation, (float) delta);
        }
        
        Character.GlobalPosition = _position;
        Character.Rotation = Vector3.Up * _orientation;
    
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
        var parent = GetParent();
        if (parent is not Node3D)
        {
            return new[]{"KinematicsController must be attached to a Node3D"};
        }

        return new string[]{};
    }
}