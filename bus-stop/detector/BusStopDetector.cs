using Godot;
using System;

/// <summary>
/// Detects whether a bus stop is in range of a bus stop.
/// Changes color modulation of the parent building sprite accordingly.
/// </summary>
public partial class BusStopDetector : RayCast2D
{
    private Sprite2D _buildingSprite;
    private Color _originalColor;
    private Color _targetColor;
    private readonly Color _lightenFactor = new(1.4f, 1.4f, 1.4f, 1.0f);
    private float _lerpSpeed = 10f;

    /// <summary>
    /// Gets the bus stop node that this detector can reach.
    /// Returns null if no bus stop is currently in range.
    /// Preview bus stops are ignored.
    /// </summary>
    public Node ReachableBusStop
    {
        get
        {
            ForceRaycastUpdate();
            if (IsColliding())
            {
                var busStopArea = GetCollider() as Node;
                var busStop = busStopArea.GetParent();
                return busStop;
            }
            return null;
        }
    }

    public override void _Ready()
    {
        _buildingSprite = GetParent<Node2D>().GetNode<Sprite2D>("Sprite2D");
        _originalColor = _buildingSprite.Modulate;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (ReachableBusStop != null)
            _targetColor = _originalColor * _lightenFactor;
        else
            _targetColor = _originalColor;
        _buildingSprite.Modulate = _buildingSprite.Modulate.Lerp(_targetColor, (float)(delta * _lerpSpeed));
    }
}
