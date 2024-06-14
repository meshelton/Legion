using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Legion.Character.Movement.Steering;

public partial class SteeringWorld3D : Node
{
    private readonly HashSet<SteeringController3D> _avoidanceEnabledCharacters = new();

    public void EnableAvoidance(SteeringController3D controller)
    {
        _avoidanceEnabledCharacters.Add(controller);
    }

    public void DisableAvoidance(SteeringController3D controller)
    {
        _avoidanceEnabledCharacters.Remove(controller);
    }

    public HashSet<SteeringController3D> GetControllersInRange(Vector3 worldPosition, float range)
    {
        // TODO: Add a spatial data structure here.
        return _avoidanceEnabledCharacters.ToHashSet();
    }
}
