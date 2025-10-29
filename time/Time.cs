using Godot;
using System;

/// <summary>
/// A data structure to represent in-game time in minutes.
/// </summary>
public partial class Time : Node
{
    private const uint MINUTES_PER_DAY = 1440;

    private uint _minutesElapsedInDay;
    public uint MinutesElapsedInDay
    {
        get { return _minutesElapsedInDay; }
        set { _minutesElapsedInDay = value % MINUTES_PER_DAY; }
    }
    
    public Time(uint minutesElapsedInDay)
    {
        _minutesElapsedInDay = minutesElapsedInDay % MINUTES_PER_DAY;
    }

    public void AddMinutes(uint minutesToAdd)
    {
        _minutesElapsedInDay = (_minutesElapsedInDay + minutesToAdd) % MINUTES_PER_DAY;
    }

    /// <summary>
    /// Retrieves the current time in HH:mm format.
    /// </summary>
    public string GetFormattedTimeString()
    {
        uint hours = _minutesElapsedInDay / 60;
        uint minutes = _minutesElapsedInDay % 60;
        
        return $"{hours:D2}:{minutes:D2}";
    }
}