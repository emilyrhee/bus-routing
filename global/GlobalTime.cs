using Godot;
using System;

public partial class GlobalTime : Node
{
    public int Hours = 0;
    public int Minutes = 0;

    public void IncrementTime()
    {
        Minutes++;
        if (Minutes >= 60)
        {
            Minutes = 0;
            Hours++;
            if (Hours >= 24)
                Hours = 0;
        }
    }

    public string GetFormattedTimeString()
    {
        return $"{Hours:D2}:{Minutes:D2}";
    }
}
