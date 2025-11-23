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
    private static uint _nextID = 1;

    private uint _ID;
    public uint ID
    { 
        get => _ID;
        private set => _ID = value;
    }

    /// <summary>
    /// List of bus stops that make up the route.
    /// </summary>
    public List<RoadNode> PathToTravel { get; set; }

    /// <summary>
    /// The name of the color assigned to this route opposed to the hex value.
    /// </summary>
    public string ColorName { get; private set; }

    /// <summary>
    /// The color assigned to this route. Set by hex value or Godot Color constants.
    /// </summary>
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
    /// Appends a new node to the end of the route's path and visual line.
    /// </summary>
    /// <param name="node">The Node2D to add to the path.</param>
    public void AppendNode(RoadNode node)
    {
        if (node == null) return;

        PathToTravel.Add(node);
        PathVisual?.AddPoint(node.GlobalPosition);
    }

    /// <summary>
    /// Automatically assigns a unique ID initializes the path list, and
    /// assigns a color.
    /// </summary>
    public Route()
    {
        ID = _nextID++;
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
            Color = Colors.White;
        }
    }
}