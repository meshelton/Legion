using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/arrive.svg")]
public partial class VelocityMatch : SteeringBehavior3D
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

    [Export]
    public float MaxAcceleration;

    [Export]
    public float TimeToTarget = 0.1f;

    public override void _Ready()
    {
        AddChild(_tracker);
    }

    public override SteeringOutput3D GetSteering()
    {
        SteeringOutput3D result = new();

        result.Linear = _tracker.Velocity - CharacterController.Velocity;
        result.Linear /= TimeToTarget;

        if (result.Linear.Length() > MaxAcceleration)
        {
            result.Linear = result.Linear.Normalized() * MaxAcceleration;
        }

        result.Angular = 0;
        return result;
    }
}
