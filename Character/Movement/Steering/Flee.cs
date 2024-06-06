using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/seek.svg")]
public partial class Flee : SteeringBehavior3D
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
            _tracker.Track(_target);
        }
    }

    [Export]
    public float MaxAcceleration = 10;

    public override void _Ready()
    {
        base._Ready();
        _tracker = new();
        AddChild(_tracker);
    }

    public override SteeringOutput3D GetSteering()
    {
        SteeringOutput3D result = new();

        // Get the direction to the target
        result.Linear = Character.Position - Target.Position;

        result.Linear = result.Linear.Normalized();
        result.Linear *= MaxAcceleration;

        result.Angular = 0;
        return result;
    }
}
