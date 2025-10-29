using Godot;
using System;

/// <summary>
/// A data structure to represent in-game time in minutes.
/// </summary>
public partial class Time : Node
{
    private uint _minutesElapsedInDay;
    public Time(uint minutesElapsedInDay)
    {
        _minutesElapsedInDay = minutesElapsedInDay % 1440;
    }

    public void AddMinutes(uint minutesToAdd)
    {
        _minutesElapsedInDay = (_minutesElapsedInDay + minutesToAdd) % 1440;
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