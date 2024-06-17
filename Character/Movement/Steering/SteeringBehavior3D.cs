using System;
using Godot;
using Godot.Collections;

namespace Legion.Character.Movement.Steering;

[Tool]
public abstract partial class SteeringBehavior3D : Node3D
{
    [Export]
    public float BlendingWeight = 1;

    public Node3D Character
    {
        get => CharacterController.Character;
    }

    public SteeringController3D CharacterController
    {
        get
        {
            Node p = GetParent();
            return p switch
            {
                SteeringController3D sc => sc,
                SteeringBehavior3D sb => sb.CharacterController,
                null => null,
                _
                    => throw new Exception(
                        $"Not attached to a correct legion node, attached to {p.Name}"
                    )
            };
        }
    }

    public SteeringWorld3D SteeringWorld3D
    {
        get => CharacterController.World;
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
        if (parent is not (SteeringController3D or SteeringBehavior3D))
        {
            return new[] { "This must be attached to a SteeringController or another behavior" };
        }

        return System.Array.Empty<string>();
    }

    public override void _ValidateProperty(Dictionary property)
    {
        if (property["name"].AsString() == "BlendingWeight")
        {
            long usageHint = GetParent() is not BlendedSteering3D
                ? (long)PropertyUsageFlags.NoEditor
                : property["usage"].AsInt64();

            property["usage"] = Variant.From(usageHint);
        }

        base._ValidateProperty(property);
    }

    public abstract SteeringOutput3D GetSteering();
}
