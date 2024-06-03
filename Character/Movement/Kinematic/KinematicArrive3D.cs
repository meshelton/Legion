using Godot;

namespace Legion.Character.Movement.Kinematic;

[Tool]
[GlobalClass, Icon("res://Icons/arrive.svg")]
public partial class KinematicArrive3D : KinematicBehavior3D
{
    [Export] public Node3D Target;
    [Export] public float MaxSpeed;
    [Export] public float ArrivalRadius;
    [Export] public float TimeToTarget;

    public override KinematicSteeringOutput3D GetSteering()
    {
        KinematicSteeringOutput3D result = new();
        
        // Get the direction to the target
        result.Velocity = Target.Position - Character.Position;
        
        // Check if already within radius from target
        if (result.Velocity.Length() < ArrivalRadius)
        {
            // This might be better as null...
            return result;
        }
        
        // Would like to get to the target in TimeToTargetSeconds
        result.Velocity /= TimeToTarget;
        
        // If it's too fast, clip to max speed
        if (result.Velocity.Length() > MaxSpeed)
        {
            result.Velocity = result.Velocity.Normalized() * MaxSpeed;
        }

        // Face the direction we want to move;
        Character.LookAt(result.Velocity, useModelFront: true);
        result.Rotation = 0;
        return result;
    }
}