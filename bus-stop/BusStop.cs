using Godot;
using System;
using System.Collections.Generic;

public partial class BusStop : RoadNode
{
    public List<RoadEdge> ConnectedEdges = [];

    public override void _Ready()
    {
        base._Ready(); // Unsure if I need this. Look into it later.
        LevelState.AllBusStops.Add(this);
    }
}
