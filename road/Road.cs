using Godot;
using System;

public partial class Road : Path2D
{
    [Export]
    public NodePath RoadStroke = "RoadStroke";

    private Line2D roadStroke;

    private void DrawLine(Line2D stroke)
    {
        Vector2[] points = Curve.GetBakedPoints();
        stroke.Points = points;
    }
    
    public override void _Ready()
    {
        roadStroke = GetNode<Line2D>(RoadStroke);
        DrawLine(roadStroke);
    }
}