using Godot;
using Godot.Collections;

namespace Legion.Character.Movement;

[Tool]
[GlobalClass, Icon("res://Icons/kinematics.svg")]
public partial class KinematicController3D : Node
{
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

        _position = GetParent<Node3D>().Position;
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
        {
            return;
        }
        
        Array<KinematicMovement3D> movements = this.GetChildrenOfType<KinematicMovement3D>();

        foreach (KinematicMovement3D movement in movements)
        {
            var steering = movement.GetSteering();
            Update(steering, (float) delta);
        }
        
        GetParent<Node3D>().Position = _position;
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
        var parent = GetParent();
        if (parent is not Node3D)
        {
            return new[]{"KinematicsController must be attached to a Node3D"};
        }

        return new string[]{};
    }
}