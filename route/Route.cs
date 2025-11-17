using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents a route consisting of a sequence of bus stops and its visual
/// representation.
/// </summary>
public partial class Route : Node
{
    /// <summary>
    /// A static counter to ensure every new route gets a unique ID.
    /// </summary>
    private static uint _nextId = 1;

    private uint _ID;
    public uint ID
    { 
        get => _ID;
        private set => _ID = value;
    }

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
        ID = _nextId++;
        PathToTravel = [];
    }
}