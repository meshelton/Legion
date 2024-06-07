using Godot;
using Godot.Collections;

namespace Legion.Character.Movement;

public static class Extensions
{
    // A counterclockwise angle, in radians, from the positive z-axis
    public static float Orientation(this Node3D node)
    {
        return Vector3.ModelFront.SignedAngleTo(node.OrientationVector(), Vector3.Up);
    }

    public static void SetOrientation(this Node3D node, float orientation)
    {
        node.Rotation = Vector3.Up * orientation;
    }

    public static Vector3 OrientationVector(this Node3D node)
    {
        return node.Basis.Z.Normalized();
    }

    public static float RandBinom(this RandomNumberGenerator rng)
    {
        return rng.Randf() - rng.Randf();
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
