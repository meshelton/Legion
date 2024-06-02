using Godot;

namespace Legion.Character.Movement;

[Tool]
[GlobalClass, Icon("res://Icons/arrive.svg")]
public partial class KinematicArrive3D : KinematicMovement3D
{
    [Export] public Node3D Target;
    [Export] public float MaxSpeed;
    [Export] public float ArrivalRadius;
    [Export] public float TimeToTarget;

    public override KinematicSteeringOutput3D GetSteering()
    {
        KinematicSteeringOutput3D result = new();
        // Get the vector to target
        Vector3 vectorToTarget = Target.Position - Character.Position;
        if (vectorToTarget.Length() < ArrivalRadius)
        {
            return result;
        }
        Vector3 desiredVelocity = vectorToTarget / TimeToTarget;
        if (desiredVelocity.Length() > MaxSpeed)
        {
            desiredVelocity = desiredVelocity.Normalized() * MaxSpeed;
        }

        result.Velocity = desiredVelocity;
        Character.LookAt(desiredVelocity);
        result.Rotation = 0;
        return result;
    }
}