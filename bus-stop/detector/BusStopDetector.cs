using Godot;
using System;

public partial class BusStopDetector : RayCast2D
{
    private Sprite2D _buildingSprite;
    private Color _originalColor;
    private readonly Color _darkenFactor = new(0.5f, 0.5f, 0.5f, 1.0f);

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
                if (LevelState.AllBusStops.Contains(busStop)) // ensures preview bus stops are ignored
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
        {
            _buildingSprite.Modulate = _originalColor * _darkenFactor;
        }
        else
        {
            _buildingSprite.Modulate = _originalColor;
        }
    }

}
