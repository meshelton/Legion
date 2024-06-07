using System;
using Godot;
using Legion.Objects;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/wander.svg")]
public partial class Wander : Face
{
    private DebugDelegatedTarget _delegatedTarget;

    [Export]
    public float WanderOffset = 3.0f;

    [Export]
    public float WanderRadius = 1.0f;

    [Export]
    public float WanderRate = (float)Math.PI / 4.0f;

    [Export]
    public float WanderOrientation;

    [Export]
    public float MaxAcceleration = 2.0f;

    private RandomNumberGenerator _rng = new();

    public override void _Ready()
    {
        base._Ready();
        _delegatedTarget = new();
        AddChild(_delegatedTarget);
        Target = _delegatedTarget;
    }

    public override SteeringOutput3D GetSteering()
    {
        WanderOrientation += _rng.RandBinom() * WanderRate;

        float targetOrientation = WanderOrientation + Character.Orientation();

        _delegatedTarget.GlobalPosition =
            Character.GlobalPosition + WanderOffset * Character.OrientationVector();

        _delegatedTarget.GlobalPosition +=
            WanderRadius * Vector3.ModelFront.Rotated(Vector3.Up, targetOrientation);

        SteeringOutput3D result = base.GetSteering();

        result.Linear = MaxAcceleration * Character.OrientationVector();

        return result;
    }
}
