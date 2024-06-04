using Godot;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/wander.svg")]
public partial class Wander : Face
{
    private Node3D _delegatedTarget = new();

    [Export]
    public float WanderOffset;

    [Export]
    public float WanderRadius;

    [Export]
    public float WanderRate;

    [Export]
    public float WanderOrientation;

    [Export]
    public float MaxAcceleration;

    private RandomNumberGenerator _rng = new();

    public override void _Ready()
    {
        AddChild(_delegatedTarget);
        base.Target = _delegatedTarget;
        base._Ready();
    }

    public override SteeringOutput3D GetSteering()
    {
        WanderOrientation += _rng.RandBinom() * WanderRate;

        var targetOrientation = WanderOrientation + CharacterController.Orientation;

        _delegatedTarget.Position =
            CharacterController.Position + WanderOffset * CharacterController.OrientationVector;

        _delegatedTarget.Position +=
            WanderRadius * Vector3.Forward.Rotated(Vector3.Up, targetOrientation);

        var result = base.GetSteering();

        result.Linear = MaxAcceleration * CharacterController.OrientationVector;

        return result;
    }
}
