#if TOOLS
using Godot;

namespace Legion.addons.LegionAI;

[Tool]
public partial class LegionAI : EditorPlugin
{
    private EditorNode3DGizmoPlugin _wanderInterfacePlugin;
    private BlendingPlugin _blendingPlugin;

    public override void _EnterTree()
    {
        _wanderInterfacePlugin = new WanderInterface();
        _blendingPlugin = new BlendingPlugin();

        // Initialization of the plugin goes here.
        AddNode3DGizmoPlugin(_wanderInterfacePlugin);
        AddInspectorPlugin(_blendingPlugin);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveNode3DGizmoPlugin(_wanderInterfacePlugin);
        RemoveInspectorPlugin(_blendingPlugin);
    }
}

#endif
