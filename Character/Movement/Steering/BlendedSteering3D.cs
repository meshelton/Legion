using System.Linq;
using Godot;
using Godot.Collections;

namespace Legion.Character.Movement.Steering;

[GlobalClass]
[Icon("res://Icons/blended.svg")]
[Tool]
public partial class BlendedSteering3D : SteeringBehavior3D
{
    public static string HintString = "ChildBlendingNodeEditor";

    public override SteeringOutput3D GetSteering()
    {
        return this.GetChildrenOfType<SteeringBehavior3D>()
            .Aggregate(
                new SteeringOutput3D(),
                (result, behavior) => result + behavior.GetSteering() * behavior.BlendingWeight
            );
    }

    public override string[] _GetConfigurationWarnings()
    {
        Array<SteeringBehavior3D> children = this.GetChildrenOfType<SteeringBehavior3D>();
        if (children.Count == 0)
        {
            return new[] { "BlendedSteering3D needs SteeringBehavior3D children to blend" };
        }
        return System.Array.Empty<string>();
    }

    public override Array<Dictionary> _GetPropertyList()
    {
        Array<Dictionary> properties = new();

        properties.Add(
            new Dictionary
            {
                { "name", $"Blending" },
                { "type", (int)Variant.Type.Array },
                { "hint", (int)PropertyHint.None },
                { "hint_string", HintString },
            }
        );

        return properties;
    }

    public override Variant _Get(StringName property)
    {
        if (property != "Blending")
        {
            return base._Get(property);
        }

        return this.GetChildrenOfType<SteeringBehavior3D>();
    }

    public override bool _Set(StringName property, Variant value)
    {
        return property == "Blending";
    }
}
