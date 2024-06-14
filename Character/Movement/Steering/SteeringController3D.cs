using Godot;
using Godot.Collections;

namespace Legion.Character.Movement.Steering;

[Tool]
[GlobalClass]
[Icon("res://Icons/kinematics.svg")]
public partial class SteeringController3D : Node3D
{
    // TODO: Add a configuration environment that allows setting defaults for all these values
    [Export]
    public float MaxSpeed = 10;

    [Export]
    public float MaxRotation = Mathf.Pi / 2;

    [Export]
    public bool IncludeDampening = true;

    [ExportGroup("Avoidance")]
    [Export]
    public bool AvoidanceEnabled
    {
        get => _avoidanceEnabled;
        set
        {
            _avoidanceEnabled = value;
            UpdateAvoidance();
        }
    }

    private void UpdateAvoidance()
    {
        UpdateConfigurationWarnings();
        if (_avoidanceEnabled)
        {
            World?.EnableAvoidance(this);
        }
        else
        {
            World?.DisableAvoidance(this);
        }
    }

    [Export(PropertyHint.LayersAvoidance)]
    public uint AvoidanceLayers { get; set; }

    [Export(PropertyHint.LayersAvoidance)]
    public uint AvoidanceMask { get; set; }

    public SteeringWorld3D World { get; private set; }

    public Vector3 Velocity { get; private set; }
    public new float Rotation { get; private set; }

    // Might make sense to get rid of these.
    private Vector3 _position;
    private float _orientation;
    private bool _avoidanceEnabled;

    public Node3D Character
    {
        get => GetParent<Node3D>();
    }

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            // Code to execute in editor.
            UpdateConfigurationWarnings();
        }

        World = GetNodeOrNull<SteeringWorld3D>("/root/SteeringWorld3d");
        if (World == null)
        {
            World = new SteeringWorld3D();
            // Is this the right way to add an autoload.
            GetTree().Root.AddChild(World);
        }
        UpdateAvoidance();

        _position = GetParent<Node3D>().GlobalPosition;
        _orientation = GetParent<Node3D>().Orientation();
    }

    // TODO: Lack of drag force causes various steering behaviors to oscillate forever
    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint())
        {
            return;
        }

        Array<SteeringBehavior3D> movements = this.GetChildrenOfType<SteeringBehavior3D>();

        foreach (SteeringBehavior3D movement in movements)
        {
            SteeringOutput3D steering = movement.GetSteering();
            Update(steering, MaxSpeed, MaxRotation, (float)delta);
        }

        Character.GlobalPosition = _position;
        Character.SetOrientation(_orientation);
    }

    private void Update(SteeringOutput3D steering, float maxSpeed, float maxRotation, float time)
    {
        _position += Velocity * time;
        _orientation += Rotation * time;

        Velocity += steering.Linear * time;
        Rotation += steering.Angular * time;

        if (Velocity.Length() > maxSpeed)
        {
            Velocity = Velocity.Normalized();
            Velocity *= maxSpeed;
        }

        if (Rotation > maxRotation)
        {
            Rotation = maxRotation;
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        Node parent = GetParent();
        if (parent is not Node3D)
        {
            return new[] { "SteeringController3D must be attached to a Node3D" };
        }

        return new string[] { };
    }
}
