using Godot;

namespace Legion.Character.Movement;

[Tool]
public abstract partial class KinematicMovement3D : Node
{
    protected Node3D Character => GetParent<KinematicController3D>().Character;

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            UpdateConfigurationWarnings();
        }
    }
    
    public override string[] _GetConfigurationWarnings()
    {
        var parent = GetParent();
        if (parent is not KinematicController3D)
        {
            return new[]{"This must be attached to a KinematicsController"};
        }

        return new string[]{};
    }    

    public abstract KinematicSteeringOutput3D GetSteering();
}