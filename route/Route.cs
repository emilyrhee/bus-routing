using Godot;
using System;

public partial class Route : Node
{
    private ulong _routeID;
    private Godot.Collections.Array<RoadNode> _stops = [];
    public Godot.Collections.Array<RoadNode> Stops
    {
        get => _stops;
        set => _stops = value;
    }
    private Godot.Collections.Array<RoadEdge> _pathToTravel = [];
    private Time _startTime;
    public Time StartTime
    {
        get => _startTime;
        set => _startTime = value;
    }
    private uint _frequencyMinutes;

    public Route(Time startTime, uint frequencyMinutes)
    {
        _routeID = GetInstanceId();
        _startTime = startTime;
        _frequencyMinutes = frequencyMinutes;
    }
}