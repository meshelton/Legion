using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/arrive.svg")]
public partial class Arrive : SteeringBehavior3D
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
    public float MaxAcceleration = 5.0f;

    [Export]
    public float MaxSpeed = 3.0f;

    [Export]
    public float TargetRadius = 0.1f;

    [Export]
    public float SlowRadius = 0.5f;

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

        Vector3 direction = _tracker.Position - CharacterController.Position;
        float distance = direction.Length();

        // We've arrived, do nothing else;
        if (distance < TargetRadius)
        {
            // Maybe should be null;
            return result;
        }

        // Get there as quickly as possible
        float targetSpeed = MaxSpeed;
        // Unless already inside the slow radius
        if (distance <= SlowRadius)
        {
            // In which case scale speed by remaining distance to go
            targetSpeed *= distance / SlowRadius;
        }

        // Velocity is just speed with direction
        Vector3 targetVelocity = direction.Normalized() * targetSpeed;

        result.Linear = targetVelocity - CharacterController.Velocity;
        result.Linear /= TimeToTarget;

        if (result.Linear.Length() > MaxAcceleration)
        {
            result.Linear = result.Linear.Normalized() * MaxAcceleration;
        }

        result.Angular = 0;
        return result;
    }
}
