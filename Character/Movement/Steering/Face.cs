using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/face.svg")]
public partial class Face : Align
{
    private Node3D _target;
    private KinematicTracker _tracker;
    private Node3D _delegatedTarget;

    [Export]
    public new Node3D Target
    {
        get => _target;
        set
        {
            _target = value;
            _tracker?.Track(_target);
            base.Target = _delegatedTarget;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        _tracker = new();
        _tracker.Track(_target);
        _delegatedTarget = new();
        base.Target = _delegatedTarget;
        AddChild(_tracker);
        AddChild(_delegatedTarget);
    }

    public override SteeringOutput3D GetSteering()
    {
        Vector3 direction = _tracker.Position - Character.Position;

        if (direction.Length() == 0)
        {
            return SteeringOutput3D.FromTracker(_tracker);
        }

        _tracker.UpdateNode(_delegatedTarget);
        // Book gives this as -X instead. Why does it need to be X here?
        _delegatedTarget.SetOrientation(Mathf.Atan2(direction.X, direction.Z));
        return base.GetSteering();
    }
}
