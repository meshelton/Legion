using System.Collections.Generic;
using System.Linq;
using Godot;
using Legion.Character.Movement;

namespace Legion.Scripts;

[GlobalClass]
[Tool]
public partial class GlobalHerd : Node3D
{
    // TODO: Would be nice to have a better type on this, something that acknowledges that it's a group
    [Export]
    public StringName Group;

    [Export]
    public Vector3 Velocity { get; private set; }

    private Vector3 _prevGlobalPosition;

    public override void _PhysicsProcess(double delta)
    {
        Velocity = (GlobalPosition - _prevGlobalPosition) / (float)delta;

        _prevGlobalPosition = GlobalPosition;
        List<Node3D> nodes = GetTree().GetNodesInGroup(Group).OfType<Node3D>().ToList();

        if (nodes.Count == 0)
        {
            GlobalPosition = Vector3.Zero;
            Velocity = Vector3.Zero;
        }

        GlobalPosition =
            nodes.Aggregate(new Vector3(), (agg, node3D) => agg + node3D.GlobalPosition)
            / nodes.Count;

        float avgOrientation =
            nodes.Aggregate(0.0f, (f, node3D) => f + node3D.Orientation()) / nodes.Count();

        this.SetOrientation(avgOrientation);
    }
}
