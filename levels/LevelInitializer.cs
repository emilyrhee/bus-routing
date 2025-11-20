using Godot;
using System;

/// <summary>
/// This script is to be attached to the root of each level.
/// See also LevelState.cs for level state management.
/// </summary>
public partial class LevelInitializer : Node2D
{
    private void DrawRoadEdges()
    {
        var roadNodes = GetNode("RoadNodes").GetChildren();
        if (roadNodes == null)
        {
            GD.PrintErr("No RoadNodes found. Check if RoadNodes Node exists.");
            return;
        }

        var roadEdgeScene = GD.Load<PackedScene>("res://road/road-edge.tscn");
        
        foreach (Node node in roadNodes)
        {
            if (node is RoadNode roadNode)
            {
                foreach (RoadNode neighbor in roadNode.Neighbors)
                {
                    // // Only draw edge once per pair (avoid duplicates)
                    if (roadNode.GetInstanceId() < neighbor.GetInstanceId())
                    {
                        var edge = roadEdgeScene.Instantiate<RoadEdge>();
                        AddChild(edge);
                        edge.SetEndpoints(roadNode, neighbor);
                    }
                }
            }
        }
    }
    public override void _Ready()
    {
        LevelState levelState = new LevelState();
        AddChild(levelState); // unsure if this is necessary. check back later.

        DrawRoadEdges();
    }
}
