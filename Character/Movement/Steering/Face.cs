using System;
using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass, Icon("res://Icons/face.svg")]
public partial class Face : SteeringBehavior3D
{
    private Node3D _target;
    private KinematicTracker _tracker = new();

    [Export]
    public Node3D Target
    {
        get => _target;
        set
        {
            _target = value;
            _tracker.Track(_target);
        }
    }

    [Export] public float MaxAngularAcceleration;
    [Export] public float MaxRotation;

    [Export] public float TargetRadius = (float) Math.PI / 36.0f;
    [Export] public float SlowRadius = (float)Math.PI / 12.0f;
    [Export] public float TimeToTarget = 0.1f;
    

    public override void _Process(double delta)
    {
        _tracker.Update((float) delta);
    }


    public override SteeringOutput3D GetSteering()
    {
        SteeringOutput3D result = new();

        var rotation = Mathf.AngleDifference( CharacterController.Orientation, _tracker.Orientation);
        var rotationSize = Mathf.Abs(rotation);
        
        // We've arrived, do nothing else;
        if (rotationSize < TargetRadius)
        {
            // Maybe should be null;
            return result;
        }

        float targetRotation;
        if (rotationSize > SlowRadius)
        {
            targetRotation = MaxRotation;
        }
        else
        {
            targetRotation = MaxRotation * (rotationSize / SlowRadius);
        }

        targetRotation *= Mathf.Sign(rotation);

        result.Angular = targetRotation - CharacterController.Rotation;
        result.Angular /= TimeToTarget;

        var angularAcceleration = Mathf.Abs(result.Angular);
        if (angularAcceleration > MaxAngularAcceleration)
        {
            result.Angular /= angularAcceleration;
            result.Angular *= MaxAngularAcceleration;
        }

        result.Linear = Vector3.Zero;
        return result;
    }
}