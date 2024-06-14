using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Legion.Character.Movement.Steering;

[GlobalClass]
[Tool]
[Icon("res://Icons/separation.svg")]
public partial class Separation : SteeringBehavior3D
{
    [Export]
    public float MaxAcceleration = 10;

    // The threshold to take action
    [Export]
    public float Threshold = 5;

    // The constant coefficient of decay for the inverse square law.
    [Export]
    public float DecayCoefficient = 1;

    public override SteeringOutput3D GetSteering()
    {
        SteeringOutput3D result = new();

        if (!CharacterController.AvoidanceEnabled)
        {
            return result;
        }

        var targets = SteeringWorld3D
            .GetControllersInRange(Character.GlobalPosition, Threshold)
            .Where(sc =>
                sc.AvoidanceEnabled && (sc.AvoidanceLayers & CharacterController.AvoidanceMask) != 0
            );

        foreach (SteeringController3D target in targets)
        {
            Vector3 direction = CharacterController.GlobalPosition - target.GlobalPosition;
            float distance = direction.Length();
            direction = direction.Normalized() * Mathf.Sign(DecayCoefficient);
            if (distance < Threshold)
            {
                float strength = Mathf.Min(
                    Mathf.Abs(DecayCoefficient) / (distance * distance),
                    MaxAcceleration
                );
                result.Linear += strength * direction;
            }
        }

        return result;
    }

    public override string[] _GetConfigurationWarnings()
    {
        if (!CharacterController.AvoidanceEnabled)
        {
            return new[] { "Avoidance needs to be enabled this behavior's SteeringController3D" };
        }

        return new string[] { };
    }
}
