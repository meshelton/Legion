using System;
using Godot;

namespace Legion.Objects;

[GlobalClass]
[Tool]
public partial class DebugDelegatedTarget : Marker3D
{
    public override void _Ready()
    {
        PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Objects/arrow.glb");
        SyntyAsset syntyAsset = packedScene.Instantiate<SyntyAsset>();
        AddChild(syntyAsset);
        syntyAsset.Rotation = Vector3.Right * Mathf.DegToRad(90);
        syntyAsset.SyntyTexture = ResourceLoader.Load<Texture2D>(
            "res://Textures/PolygonPrototype_Texture_01.png"
        );
        syntyAsset.Scale = Vector3.One * 2;
        syntyAsset.UpdateTexture();
    }
}
