using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

[Tool]
public abstract partial class SteeringBehavior3D : Node
{
    protected Node3D Character => GetParent<SteeringController3D>().Character;

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
        if (parent is not SteeringController3D)
        {
            return new[]{"This must be attached to a SteeringController"};
        }

        return new string[]{};
    }    

    public abstract SteeringOutput3D GetSteering();
}