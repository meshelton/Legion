using System;
using Godot;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/wander.svg")]
public partial class Wander : Face
{
    private Marker3D _delegatedTarget;

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

        if (Engine.IsEditorHint())
        {
            GD.Print("Getting steering to initialize the marker");
            _delegatedTarget.Owner = GetTree().EditedSceneRoot;

            GetSteering();

            GD.Print(_delegatedTarget.Position);
        }
    }

    public override SteeringOutput3D GetSteering()
    {
        WanderOrientation += _rng.RandBinom() * WanderRate;

        float targetOrientation = WanderOrientation + CharacterController.Orientation;

        _delegatedTarget.Position =
            CharacterController.Position + WanderOffset * CharacterController.OrientationVector;

        _delegatedTarget.Position +=
            WanderRadius * -Vector3.Forward.Rotated(Vector3.Up, targetOrientation);

        SteeringOutput3D result = base.GetSteering();

        result.Linear = MaxAcceleration * CharacterController.OrientationVector;

        return result;
    }
}
