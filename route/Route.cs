using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents a route consisting of a sequence of bus stops and its visual
/// representation.
/// </summary>
public partial class Route : Node
{
    private ulong _routeID;

    /// <summary>
    /// List of bus stops that make up the route.
    /// </summary>
    public List<Node> PathToTravel { get; set; }

    /// <summary>
    /// A single Line2D node that represents the entire visual path of the route.
    /// </summary>
    public Line2D PathVisual { get; set; }

    public Route()
    {
        _routeID = GetInstanceId();
        PathToTravel = [];
    }
}