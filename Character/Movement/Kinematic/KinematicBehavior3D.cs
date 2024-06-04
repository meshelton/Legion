using Godot;

namespace Legion.Character.Movement.Kinematic;

[Tool]
public abstract partial class KinematicBehavior3D : Node
{
    protected Node3D Character
    {
        get => GetParent<KinematicController3D>().Character;
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
        if (parent is not KinematicController3D)
        {
            return new[] { "This must be attached to a KinematicsController" };
        }

        return new string[] { };
    }

    public abstract KinematicSteeringOutput3D GetSteering();
}
