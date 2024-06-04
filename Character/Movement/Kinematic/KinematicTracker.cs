using Godot;

namespace Legion.Character.Movement.Kinematic;

public partial class KinematicTracker : Node
{
    public Vector3 Position
    {
        get => _trackedNode.GlobalPosition;
    }

    public float Orientation
    {
        get => _trackedNode.Orientation();
    }

    public Vector3 Velocity
    {
        get => _velocity;
    }

    public float Rotation
    {
        get => _rotation;
    }

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

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
        {
            return;
        }

        Update((float)delta);
    }

    public void Update(float delta)
    {
        if (_trackedNode == null)
        {
            return;
        }

        float curOrientation = _trackedNode.Orientation();
        Vector3 curPosition = _trackedNode.GlobalPosition;

        _rotation = (curOrientation - _prevOrientation) / delta;
        _velocity = (curPosition - _prevPosition) / delta;

        _prevOrientation = curOrientation;
        _prevPosition = curPosition;
    }

    public void UpdateNode(Node3D node)
    {
        node.GlobalPosition = Position;
        node.SetOrientation(Orientation);
    }
}
