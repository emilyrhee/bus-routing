using Godot;
using System.Collections.Generic;

/// <summary>
/// Manages the visual representation of a route using a Line2D node.
/// </summary>
public partial class RouteVisual : Node2D
{
    private Route _route;
    private Line2D _line;
    
    /// <summary>
    /// The width of the route line.
    /// </summary>
    public float LineWidth { get; set; } = 8.0f;

    public RouteVisual(Route route)
    {
        _route = route;
        _line = new Line2D
        {
            Width = LineWidth,
            DefaultColor = route.Color
        };
        AddChild(_line);
    }

    public override void _Ready()
    {
        UpdateVisual();
    }


    /// <summary>
    /// Rebuilds the entire visual from the route's current path.
    /// </summary>
    public void UpdateVisual()
    {
        _line.ClearPoints();
        foreach (var node in _route.Path)
        {
            _line.AddPoint(node.GlobalPosition);
        }
    }

    /// <summary>
    /// Adds a point to the end of the visual line.
    /// </summary>
    public void AppendPoint(Vector2 position)
    {
        _line.AddPoint(position);
    }

    /// <summary>
    /// Adds a point to the beginning of the visual line.
    /// </summary>
    public void PrependPoint(Vector2 position)
    {
        _line.AddPoint(position, 0);
    }

    /// <summary>
    /// Clears all points from the visual line.
    /// </summary>
    public void ClearPoints()
    {
        _line.ClearPoints();
    }

    /// <summary>
    /// Gets the Line2D node for direct manipulation if needed.
    /// </summary>
    public Line2D GetLine2D()
    {
        return _line;
    }
}