using Godot;
using System;

public partial class RoadNode : Node2D
{
    [Export] public Godot.Collections.Array<Node2D> Neighbors = [];

    public override void _Ready()
    {
        LevelState.AllRoadNodes.Add(this);
    }
}
