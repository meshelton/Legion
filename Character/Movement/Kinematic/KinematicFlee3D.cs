using Godot;

namespace Legion.Character.Movement.Kinematic;

[Tool]
[GlobalClass]
[Icon("res://Icons/flee.svg")]
public partial class KinematicFlee3D : KinematicBehavior3D
{
    [Export]
    public Node3D Target;

    [Export]
    public float MaxSpeed;

    public override KinematicSteeringOutput3D GetSteering()
    {
        KinematicSteeringOutput3D result = new();

        // Get the direction
        Vector3 directionToTarget = (Character.Position - Target.Position).Normalized();

        //Get there as fast as possible
        result.Velocity = directionToTarget * MaxSpeed;

        // Face where we want to move
        Character.LookAt(directionToTarget);
        result.Rotation = 0;
        return result;
    }
}
