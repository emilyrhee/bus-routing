using Godot;
using System;

public partial class RoadNode : Node2D
{
    [Export] public Godot.Collections.Array<Node2D> Neighbors = [];

    public void AddNeighbor(Node2D neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
        }
    }

    public void RemoveNeighbor(Node2D neighbor)
    {
        if (Neighbors.Contains(neighbor))
        {
            Neighbors.Remove(neighbor);
        }
    }

    public override void _Ready()
    {
        LevelState.AllRoadNodes.Add(this);
    }
}
