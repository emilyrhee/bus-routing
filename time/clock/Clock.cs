using Godot;
using System;

public partial class Clock : Node
{
    private RichTextLabel _label;
    private GlobalTime _globalTime;

    public override void _Ready()
    {
        _label = GetNode<RichTextLabel>("RichTextLabel");
        _globalTime = GetNode<GlobalTime>("/root/GlobalTime");
        _globalTime.TimeChanged += OnGlobalTimeChanged;
        UpdateLabel();
    }

    private void OnGlobalTimeChanged()
    {
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        _label.Text = _globalTime.GetFormattedTimeString();
    }
}