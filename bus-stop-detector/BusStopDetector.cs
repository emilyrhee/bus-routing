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
    /// </summary>
    public Node ReachableBusStop
    {
        get
        {
            ForceRaycastUpdate();
            if (IsColliding())
            {
                var busStop = GetCollider() as Node;
                return busStop?.GetParent();
            }
            return null;
        }
    }

    public override void _Ready()
    {
        _buildingSprite = GetParent<Node2D>().GetNode<Sprite2D>("Sprite2D");
        _originalColor = _buildingSprite.Modulate;
    }

    public override void _Process(double delta)
    {
        if (ReachableBusStop != null
        && EditorState.ActiveTool == EditorTool.AddDeleteBusStop)
        {
            _buildingSprite.Modulate = _originalColor * _darkenFactor;
        }
        else
        {
            _buildingSprite.Modulate = _originalColor;
        }
    }

}
