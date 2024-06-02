using Godot;

namespace Legion.Character.Movement.Kinematic;

[Tool]
[GlobalClass, Icon("res://Icons/wander.svg")]
public partial class KinematicWander : KinematicMovement3D
{
    [Export] public float MaxSpeed;
    [Export] public float MaxRotation;

    private RandomNumberGenerator _rng = new();
    
    public override KinematicSteeringOutput3D GetSteering()
    {
        KinematicSteeringOutput3D result = new();

        result.Velocity = MaxSpeed * Character.OrientationVector();
        result.Rotation = _rng.RandBinom() * MaxRotation;

        return result;

    }
}