using Godot;
using Godot.Collections;

namespace Legion.Character.Movement;

public static class Extensions
{

    public static float Orientation(this Node3D node)
    {
        return node.Transform.Basis.Z.AngleTo(Vector3.Forward);
    }
    
    public static Array<T> GetChildrenOfType<[MustBeVariant] T>(this Node parent)
        where T : Node
    {
        Array<T> result = new();
        foreach (Node child in parent.GetChildren())
        {
            if (child is T childWithType)
            {
                result.Add(childWithType);
            }
        }

        return result;
    }    
    
}