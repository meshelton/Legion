using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/align.svg")]
public partial class Align : SteeringBehavior3D
{
    private Node3D _target;
    private KinematicTracker _tracker;

    [Export]
    public Node3D Target
    {
        get => _target;
        set
        {
            _target = value;
            _tracker?.Track(_target);
        }
    }

    [Export]
    public float MaxAngularAcceleration = 100;

    [Export]
    public float MaxRotation = Mathf.DegToRad(30.0f);

    [Export]
    public float TargetRadius = Mathf.DegToRad(2.0f);

    [Export]
    public float SlowRadius = Mathf.DegToRad(20.0f);

    [Export]
    public float TimeToTarget = 0.1f;

    public override void _Ready()
    {
        base._Ready();
        _tracker = new();
        _tracker.Track(_target);
        AddChild(_tracker);
    }

    public override SteeringOutput3D GetSteering()
    {
        SteeringOutput3D result = new();

        float rotation = Mathf.AngleDifference(
            CharacterController.Orientation,
            _tracker.Orientation
        );
        float rotationSize = Mathf.Abs(rotation);

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

        float angularAcceleration = Mathf.Abs(result.Angular);
        if (angularAcceleration > MaxAngularAcceleration)
        {
            result.Angular /= angularAcceleration;
            result.Angular *= MaxAngularAcceleration;
        }

        result.Linear = Vector3.Zero;
        return result;
    }
}
