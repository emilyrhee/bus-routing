using Godot;
using System;

public partial class Route : Node
{
    private ulong _routeID;

    private Godot.Collections.Array<RoadEdge> _pathToTravel = new Godot.Collections.Array<RoadEdge>();
    public Godot.Collections.Array<RoadEdge> PathToTravel
    {
        get => _pathToTravel;
        set => _pathToTravel = value;
    }

    private uint _frequencyMinutes;

    public Route()
    {
        _routeID = GetInstanceId();
    }
}