using Godot;
using System;

public partial class Clock : Node
{
    private Timer timer;
    private RichTextLabel label;
    private GlobalTime globalTime;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        label = GetNode<RichTextLabel>("RichTextLabel");
        globalTime = GetNode<GlobalTime>("/root/GlobalTime");
        UpdateLabel();
    }

    private void OnTimerTimeout()
    {
        globalTime.IncrementTime();
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        label.Text = globalTime.GetFormattedTimeString();
    }
}
