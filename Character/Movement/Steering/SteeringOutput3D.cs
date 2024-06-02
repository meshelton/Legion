using Godot;

namespace Legion.Character.Movement.Steering;

public record struct SteeringOutput3D(Vector3 Linear, float Angular);