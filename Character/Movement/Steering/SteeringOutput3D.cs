using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

public record struct SteeringOutput3D(Vector3 Linear, float Angular)
{
    public static SteeringOutput3D FromTracker(KinematicTracker tracker)
    {
        return new SteeringOutput3D(tracker.Velocity, tracker.Rotation);
    }
}
