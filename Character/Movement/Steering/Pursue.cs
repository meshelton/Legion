using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/pursue.svg")]
public partial class Pursue : Arrive
{
    private Node3D _target;
    private KinematicTracker _tracker = new();
    private Node3D _delegatedTarget = new();

    [Export]
    public new Node3D Target
    {
        get => _target;
        set
        {
            _target = value;
            _tracker.Track(_target);
            base.Target = _delegatedTarget;
        }
    }

    [Export]
    public float MaxPrediction;

    public override void _Ready()
    {
        AddChild(_delegatedTarget);
        AddChild(_tracker);
    }

    public override SteeringOutput3D GetSteering()
    {
        Vector3 direction = _tracker.Position - CharacterController.Position;
        float distance = direction.Length();

        float speed = CharacterController.Velocity.Length();

        float prediction;
        if (speed <= distance / MaxPrediction)
        {
            prediction = MaxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        _tracker.UpdateNode(_delegatedTarget);
        _delegatedTarget.Position += _tracker.Velocity * prediction;

        return base.GetSteering();
    }
}
