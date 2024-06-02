using Godot;

namespace Legion.Character.Movement;

[Tool]
[GlobalClass, Icon("res://Icons/seek.svg")]
public partial class KinematicSeek3D : KinematicMovement3D
{
    [Export] public Node3D Target;
    [Export] public float MaxSpeed;

    public override KinematicSteeringOutput3D GetSteering()
    {
        KinematicSteeringOutput3D result = new();
        
        // Get the direction
        Vector3 directionToTarget = (Target.Position - Character.Position).Normalized();
        
        //Get there as fast as possible
        result.Velocity = directionToTarget * MaxSpeed;
        
        // Face where we want to move
        Character.LookAt(directionToTarget, useModelFront: true);
        result.Rotation = 0;
        return result;
    }
}