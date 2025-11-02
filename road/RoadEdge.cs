using Godot;
using System;

public partial class RoadEdge: Path2D
{
    [Export] private NodePath _roadStrokePath = "RoadStroke";

    private Line2D _roadStroke;

    private void DrawLine(Line2D stroke)
    {
        Vector2[] points = Curve.GetBakedPoints();
        stroke.Points = points;
    }
    
    public override void _Ready()
    {
        _roadStroke = GetNode<Line2D>(_roadStrokePath);
        DrawLine(_roadStroke);
    }
}