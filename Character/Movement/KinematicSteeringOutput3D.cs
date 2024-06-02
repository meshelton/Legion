using Godot;

namespace Legion.Character.Movement;

public record struct KinematicSteeringOutput3D(Vector3 Velocity, float Rotation);