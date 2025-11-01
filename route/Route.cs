using Godot;
using System;

public partial class Route : Node
{
    private ulong _routeID;
    private Godot.Collections.Array<RoadNode> _waypoints = new Godot.Collections.Array<RoadNode>();
    public Godot.Collections.Array<RoadNode> Waypoints
    {
        get => _waypoints;
        set => _waypoints = value;
    }

    // make path edges publicly settable so scheduler can assign edges to follow
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