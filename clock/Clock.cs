using Godot;
using System;

public partial class Clock : Node
{
    private Timer timer;
    private RichTextLabel label;
    private int hours = 0;
    private int minutes = 0;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        label = GetNode<RichTextLabel>("RichTextLabel");
        UpdateLabel();
    }

    private void OnTimerTimeout()
    {
        minutes++;
        if (minutes >= 60)
        {
            minutes = 0;
            hours++;
            if (hours >= 24)
                hours = 0;
        }
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        label.Text = $"{hours:D2}:{minutes:D2}"; // format shows as HH:MM
    }
}
