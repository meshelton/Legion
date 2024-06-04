using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/face.svg")]
public partial class Face : Align
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

    public override void _Ready()
    {
        AddChild(_tracker);
        AddChild(_delegatedTarget);
    }

    public override SteeringOutput3D GetSteering()
    {
        Vector3 direction = _tracker.Position - CharacterController.Position;

        if (direction.Length() == 0)
        {
            return SteeringOutput3D.FromTracker(_tracker);
        }

        _tracker.UpdateNode(_delegatedTarget);
        _delegatedTarget.SetOrientation(Mathf.Atan2(direction.X, direction.Z));
        return base.GetSteering();
    }
}
