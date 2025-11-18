using Godot;
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

    public string ColorName { get; private set; }
    public Color Color { get; private set; }

    private Line2D _pathVisual;
    /// <summary>
    /// A single Line2D node that represents the entire visual path of the route.
    /// </summary>
    public Line2D PathVisual
    {
        get => _pathVisual;
        set
        {
            _pathVisual = value;
            _pathVisual.Width = 8.0f;
            _pathVisual.DefaultColor = Color;
        }
    }

    /// <summary>
    /// Automatically assigns a unique ID initializes the path list, and
    /// assigns a color.
    /// </summary>
    public Route()
    {
        ID = _nextId++;
        PathToTravel = [];
        var colorInfo = LevelState.GetNextRouteColor();
        if (colorInfo.HasValue)
        {
            ColorName = colorInfo.Value.Key;
            Color = colorInfo.Value.Value;
        }
        else
        {
            // Fallback if no colors are left. TODO: make it so players cannot
            // create more routes.
            ColorName = "Default";
            Color = Godot.Colors.White;
        }
    }
}