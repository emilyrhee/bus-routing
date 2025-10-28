using Godot;
using System;

/// <summary>
/// Manages in-game time by incrementing minutes
/// </summary>
public partial class GlobalTime : Node
{
    [Signal] public delegate void TimeChangedEventHandler();

    private Timer _timer;

    public int MinutesElapsedInDay = 0; 
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
        IncrementTime();
    }

    /// <summary>
    /// Advances the game time by one minute (by default) and handles the daily wrap-around.
    /// Emits TimeChanged after updating.
    /// </summary>
    public void IncrementTime(int minutesToAdd = 1)
    {
        MinutesElapsedInDay = (MinutesElapsedInDay + minutesToAdd) % TOTAL_MINUTES_IN_DAY;
        EmitSignal(nameof(TimeChanged));
    }   

    /// <summary>
    /// Retrieves the current time in HH:mm format. Useful for displaying time in the UI.
    /// </summary>
    public string GetFormattedTimeString()
    {
        int hours = MinutesElapsedInDay / 60;
        int minutes = MinutesElapsedInDay % 60;
        
        return $"{hours:D2}:{minutes:D2}";
    }
}