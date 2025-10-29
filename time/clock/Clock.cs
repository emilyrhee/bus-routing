using Godot;
using System;

/// <summary>
/// Deals with the display of in-game time to the UI.
/// </summary>
public partial class Clock : Node
{
    private RichTextLabel _label;
    private GlobalTime _globalTime;

    public override void _Ready()
    {
        _label = GetNode<RichTextLabel>("RichTextLabel");
        _globalTime = GetNode<GlobalTime>("/root/GlobalTime");
        _globalTime.TimeAdvanced += OnGlobalTimeAdvanced;
        UpdateLabel();
    }

    private void OnGlobalTimeAdvanced()
    {
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        _label.Text = _globalTime.GameTime.GetFormattedTimeString();
    }
}