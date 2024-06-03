using Godot;

namespace Legion.Character.Movement.Kinematic;

public class KinematicTracker
{
    public Vector3 Position => _trackedNode.Position;

    public float Orientation => _trackedNode.Orientation();

    public Vector3 Velocity => _velocity;

    public float Rotation => _rotation;

    private Vector3 _prevPosition;
    private Vector3 _velocity;
    private float _prevOrientation;
    private float _rotation;
    private Node3D _trackedNode;

    public void Track(Node3D node)
    {
        _velocity = Vector3.Zero;
        _rotation = 0;
        _trackedNode = node;
    }

    public void Update(float delta)
    {
        if (_trackedNode == null)
        {
            return;
        }
        
        float curOrientation = _trackedNode.Orientation();
        Vector3 curPosition = _trackedNode.Position;

        _rotation = (curOrientation - _prevOrientation) / delta;
        _velocity = (curPosition - _prevPosition) / delta;

        _prevOrientation = curOrientation;
        _prevPosition = curPosition;
    }

}