using Godot;
using System.Collections.Generic;

public partial class RoadEdge: Area2D
{
    private CollisionShape2D _collisionShape;
    public CollisionShape2D CollisionShape => _collisionShape;
    private SegmentShape2D _segmentShape;

    public Vector2 A
    {
        get => _segmentShape?.A ?? Vector2.Zero;
        set
        {
            if (_segmentShape != null)
                _segmentShape.A = value;
        }
    }

    public Vector2 B
    {
        get => _segmentShape?.B ?? Vector2.Zero;
        set
        {
            if (_segmentShape != null)
                _segmentShape.B = value;
        }
    }

    public void SetEndpoints(Node2D nodeA, Node2D nodeB)
    {
        A = nodeA.GlobalPosition;
        B = nodeB.GlobalPosition;
    }

    public override void _Ready()
    {
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _segmentShape = _collisionShape.Shape as SegmentShape2D;
    }
}