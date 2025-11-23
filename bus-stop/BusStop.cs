using Godot;
using System;

public partial class BusStop : RoadNode
{
    public override void _Ready()
    {
        base._Ready(); // Unsure if I need this. Look into it later.
        LevelState.AllBusStops.Add(this);
    }
}
