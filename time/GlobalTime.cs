using Godot;
using System;

/// <summary>
/// Updates and manages the global in-game time.
public partial class GlobalTime : Node
{
    [Signal] public delegate void TimeChangedEventHandler();

    private Timer _timer;

    public Time GameTime = new Time(0);
    private const int TOTAL_MINUTES_IN_DAY = 1440; 
    
    public override void _Ready()
    {
        _timer = new Timer
        {
            WaitTime = 1.0f,
            OneShot = false,
            Autostart = true
        };
        AddChild(_timer);
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        IncrementGameTime();
    }

    /// <summary>
    /// Advances the game time by one minute (by default) and handles the daily wrap-around.
    /// </summary>
    public void IncrementGameTime(uint minutesToAdd = 1)
    {
        GameTime.AddMinutes(minutesToAdd);
        EmitSignal(nameof(TimeChanged));
    }
}