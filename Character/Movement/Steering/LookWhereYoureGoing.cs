using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/look_where_your_going.svg")]
public partial class LookWhereYoureGoing : Align
{
    private Node3D _target;

    private Marker3D _delegatedTarget;

    [Export]
    public new Node3D Target
    {
        get => _target;
        set
        {
            _target = value;
            base.Target = _delegatedTarget;
        }
    }

    public override void _Ready()
    {
        _delegatedTarget = new();
        AddChild(_delegatedTarget);
        base._Ready();
    }

    public override SteeringOutput3D GetSteering()
    {
        Vector3 direction = CharacterController.Velocity;

        if (direction.Length() == 0)
        {
            return new();
        }
        _delegatedTarget.SetOrientation(Mathf.Atan2(direction.X, direction.Z));

        return base.GetSteering();
    }
}
