using Godot;

namespace Legion.Objects;

[GlobalClass]
[Tool]
public partial class SyntyAsset : Node3D
{
    private Texture2D _syntyTexture;

    private MeshInstance3D _syntyMeshInstance3D;
    private StandardMaterial3D _syntyMaterial;

    [Export]
    public Texture2D SyntyTexture
    {
        get => _syntyTexture;
        set
        {
            _syntyTexture = value;
            if (_syntyMeshInstance3D != null)
            {
                UpdateTexture();
            }
        }
    }

    public override void _Ready()
    {
        _syntyMeshInstance3D = GetChild<MeshInstance3D>(0);
        UpdateTexture();
    }

    public void UpdateTexture()
    {
        if (_syntyTexture == null)
        {
            return;
        }
        _syntyMaterial ??= new StandardMaterial3D();
        _syntyMaterial.AlbedoTexture = _syntyTexture;
        _syntyMeshInstance3D.MaterialOverride = _syntyMaterial;
    }
}
