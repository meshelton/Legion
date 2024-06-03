using Godot;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass, Icon("res://Icons/seek.svg")]
public partial class Seek : SteeringBehavior3D
{
    [Export] public Node3D Target;
    [Export] public float MaxAcceleration = 10;
    
    public override SteeringOutput3D GetSteering()
    {
        SteeringOutput3D result = new();

        // Get the direction to the target
        result.Linear = Target.Position - Character.Position;

        result.Linear = result.Linear.Normalized();
        result.Linear *= MaxAcceleration;

        result.Angular = 0;
        return result;
    }
}