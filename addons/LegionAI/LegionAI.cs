#if TOOLS
using Godot;

namespace Legion.addons.legionai;

[Tool]
public partial class LegionAI : EditorPlugin
{
    private EditorNode3DGizmoPlugin _gizmoPlugin;

    public override void _EnterTree()
    {
        _gizmoPlugin = new WanderInterface();

        // Initialization of the plugin goes here.
        AddNode3DGizmoPlugin(_gizmoPlugin);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveNode3DGizmoPlugin(_gizmoPlugin);
    }
}
#endif
