using Godot;

namespace Legion.Character.Movement;

public record struct SteeringOutput3D(Vector3 Linear, float Angular);