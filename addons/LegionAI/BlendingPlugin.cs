using Godot;
using Legion.Character.Movement.Steering;

#if TOOLS
namespace Legion.addons.LegionAI;

public partial class BlendingPlugin : EditorInspectorPlugin
{
    public override bool _CanHandle(GodotObject @object)
    {
        return @object is BlendedSteering3D;
    }

    public override bool _ParseProperty(
        GodotObject @object,
        Variant.Type type,
        string name,
        PropertyHint hintType,
        string hintString,
        PropertyUsageFlags usageFlags,
        bool wide
    )
    {
        if (type == Variant.Type.Array && hintString == BlendedSteering3D.HintString)
        {
            AddPropertyEditor(name, new BlendingEditor());
            return true;
        }

        return false;
    }
}

#endif
