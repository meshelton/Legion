using Godot;

namespace Legion.Character.Movement.Steering;

[Tool]
public abstract partial class SteeringBehavior3D : Node3D
{
    public Node3D Character
    {
        get => CharacterController.Character;
    }

    public SteeringController3D CharacterController
    {
        get => GetParent<SteeringController3D>();
    }

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            UpdateConfigurationWarnings();
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        Node parent = GetParent();
        if (parent is not SteeringController3D)
        {
            return new[] { "This must be attached to a SteeringController" };
        }

        return new string[] { };
    }

    public abstract SteeringOutput3D GetSteering();
}
