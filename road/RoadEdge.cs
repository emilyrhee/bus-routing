using Godot;
using System.Collections.Generic;

public partial class RoadEdge: Area2D
{
    [Export] private NodePath _roadStrokePath = "RoadStroke";
    private Line2D _roadStroke;

    [Export] private NodePath _roadShapePath = "CollisionShape2D";
    private CollisionShape2D _roadShape;
    [Export] private Godot.Collections.Array<Node> _nodesOnEdge;

    private void DrawLine(Line2D stroke)
    {
        if (_roadShape.Shape is SegmentShape2D segment)
        {
            var from = segment.A;
            var to = segment.B;

            stroke.ClearPoints();
            stroke.AddPoint(from);
            stroke.AddPoint(to);
        }
    }
    
    public override void _Ready()
    {
        _roadStroke = GetNode<Line2D>(_roadStrokePath);
        _roadShape = GetNode<CollisionShape2D>(_roadShapePath);

        DrawLine(_roadStroke);
    }
}