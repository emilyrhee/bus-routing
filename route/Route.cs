using Godot;
using System;
using System.Collections.Generic;

public partial class Route : Node
{
    private ulong _routeID;

    public List<Node> PathToTravel { get; set; }

    public Route()
    {
        _routeID = GetInstanceId();
        PathToTravel = [];
    }
}