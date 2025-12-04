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
    /// List of bus stops and intersection nodes that make up the route.
    /// </summary>
    public List<RoadNode> Path { get; set; }

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
            _pathVisual.Width = 8.0f; // Default width
            _pathVisual.DefaultColor = Color;
        }
    }

    public override void _Process(double delta)
    {
        // TODO: add some visual indication that this route is selected
        if (EditorState.SelectedRoute == this)
        {
            GD.Print(EditorState.SelectedRoute.ColorName + " is selected.");
        }
    }

    /// <summary>
    /// Appends a new node to the end of the route's path and visual line.
    /// </summary>
    /// <param name="node">The Node2D to add to the path.</param>
    public void AppendNode(RoadNode node)
    {
        if (node == null) return;

        Path.Add(node);
        PathVisual?.AddPoint(node.GlobalPosition);
    }

    /// <summary>
    /// Inserts a new node at the beginning of the route's path and visual line.
    /// </summary>
    /// <param name="node">The RoadNode to add to the path.</param>
    public void PrependNode(RoadNode node)
    {
        if (node == null) return;

        Path.Insert(0, node);
        PathVisual?.AddPoint(node.GlobalPosition, 0);
    }

    /// <summary>
    /// Clears all nodes from the route's path and its visual line.
    /// </summary>
    public void ClearPath()
    {
        Path.Clear();
        PathVisual?.ClearPoints();
    }

    /// <summary>
    /// Sets the route's path to a new list of nodes, updating the visual line.
    /// </summary>
    public void SetPath(List<RoadNode> newPath)
    {
        ClearPath();
        foreach (var node in newPath)
        {
            AppendNode(node);
        }
    }
    
    public bool ContainsNode(RoadNode node)
    {
        return Path.Contains(node);
    }

    /// <summary>
    /// Automatically assigns a unique ID initializes the path list, and
    /// assigns a color.
    /// </summary>
    public Route()
    {
        ID = _nextID++;
        Path = [];
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