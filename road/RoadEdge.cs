using Godot;
using System.Collections.Generic;

public partial class RoadEdge: Area2D
{
    [Export] private NodePath _roadStrokePath = "RoadStroke";
    private Line2D _roadStroke;

    [Export] private NodePath _roadShapePath = "CollisionShape2D";
    private CollisionShape2D _roadShape;
    
    public Godot.Collections.Array<Node2D> NodesOnEdge { get; private set; } = [];

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

    private void _on_area_entered(Area2D area)
    {
        var roadNode = area.GetParent<Node2D>();
        if (!NodesOnEdge.Contains(roadNode))
        {
            NodesOnEdge.Add(roadNode);
        }

        // GD.Print($"--- Nodes on Edge: {Name} ---");
        // foreach (var node in NodesOnEdge)
        // {
        //     GD.Print($"- {node.Name}");
        // }
        // GD.Print("--------------------");
    }

    public override void _Ready()
    {
        _roadStroke = GetNode<Line2D>(_roadStrokePath);
        _roadShape = GetNode<CollisionShape2D>(_roadShapePath);

        DrawLine(_roadStroke);
    }
}