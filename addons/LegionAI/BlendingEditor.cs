#if TOOLS
using System.Collections.Generic;
using System.Linq;
using Godot;
using Legion.Character.Movement.Steering;

namespace Legion.addons.LegionAI;

public partial class BlendingEditor : EditorProperty
{
    [Signal]
    public delegate void BlendingWeightSetEventHandler(SteeringBehavior3D behavior, float newValue);

    private Dictionary<SteeringBehavior3D, EditorSpinSlider> _blendingControls = new();

    private HashSet<SteeringBehavior3D> _currentBehaviors = new();

    private VBoxContainer _container = new();

    // A guard against internal changes when the property is updated.
    private bool _updating;

    public BlendingEditor()
    {
        AddChild(_container);
        SetBottomEditor(_container);
        RefreshDisplay();

        BlendingWeightSet += (behavior, value) =>
        {
            if (_updating)
            {
                GD.Print("Skipping update while stuff is happening");
                return;
            }

            behavior.BlendingWeight = value;
            EmitChanged(
                GetEditedProperty(),
                new Godot.Collections.Array(_blendingControls.Keys.ToArray())
            );
        };
    }

    private void RefreshDisplay()
    {
        foreach (SteeringBehavior3D key in _currentBehaviors)
        {
            float blendingWeight = key.BlendingWeight;
            if (_blendingControls.ContainsKey(key))
            {
                _blendingControls[key].SetValueNoSignal(blendingWeight);
            }
            else
            {
                EditorSpinSlider newEdit = new();
                newEdit.SetValueNoSignal(blendingWeight);
                newEdit.Label = key.Name;
                // Might need to dispose this manually
                newEdit.ValueChanged += newBlendingWeight =>
                {
                    EmitSignal(SignalName.BlendingWeightSet, key, (float)newBlendingWeight);
                };

                _container.AddChild(newEdit);
                AddFocusable(newEdit);
                _blendingControls[key] = newEdit;
            }
        }

        foreach ((SteeringBehavior3D key, EditorSpinSlider edit) in _blendingControls)
        {
            if (_currentBehaviors.Contains(key))
            {
                continue;
            }
            _container.RemoveChild(edit);
            _blendingControls.Remove(key);
        }
        // TODO: Reorder the control.
    }

    public override void _UpdateProperty()
    {
        _updating = true;
        _currentBehaviors = GetEditedObject()
            .Get(GetEditedProperty())
            .AsGodotArray<SteeringBehavior3D>()
            .ToHashSet();
        RefreshDisplay();
        _updating = false;
    }
}
#endif
