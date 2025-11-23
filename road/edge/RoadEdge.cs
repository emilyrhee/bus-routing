using Godot;
using System.Collections.Generic;

public partial class RoadEdge: Area2D
{
    private CollisionShape2D _collisionShape;
    public CollisionShape2D CollisionShape => _collisionShape;
    private SegmentShape2D _segmentShape;

    public RoadNode NodeA { get; private set; }
    public RoadNode NodeB { get; private set; }

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

    /// <summary>
    /// Sets the endpoints of the road edge, both in terms of position and
    /// associated nodes.
    /// </summary>
    public void SetEndpoints(RoadNode nodeA, RoadNode nodeB)
    {
        NodeA = nodeA;
        NodeB = nodeB;
        A = nodeA.GlobalPosition;
        B = nodeB.GlobalPosition;
    }

    public override void _Ready()
    {
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _segmentShape = _collisionShape.Shape as SegmentShape2D;
    }
}