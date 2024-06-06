using Godot;

namespace Legion.Objects;

[Tool]
public partial class Cube : MeshInstance3D
{
    private Color _color = Colors.White;
    private StandardMaterial3D _material = new();

    [Export]
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            _material ??= new StandardMaterial3D();

            _material.AlbedoColor = _color;
            MaterialOverride = _material;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MaterialOverride = _material;
    }
}
