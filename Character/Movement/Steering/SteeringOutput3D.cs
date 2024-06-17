using Godot;
using Legion.Character.Movement.Kinematic;

namespace Legion.Character.Movement.Steering;

public record struct SteeringOutput3D(Vector3 Linear, float Angular)
{
    public static SteeringOutput3D FromTracker(KinematicTracker tracker)
    {
        return new SteeringOutput3D(tracker.Velocity, tracker.Rotation);
    }

    public static SteeringOutput3D operator *(float mult, SteeringOutput3D output3D)
    {
        return new SteeringOutput3D(output3D.Linear * mult, output3D.Angular * mult);
    }

    public static SteeringOutput3D operator *(SteeringOutput3D output3D, float mult)
    {
        return new SteeringOutput3D(output3D.Linear * mult, output3D.Angular * mult);
    }

    public static SteeringOutput3D operator +(SteeringOutput3D a, SteeringOutput3D b)
    {
        return new SteeringOutput3D(a.Linear + b.Linear, a.Angular + b.Angular);
    }
}
