using Godot;
using System;

/// <summary>
/// This script is to be attached to the root of each level.
/// See also <seealso cref="LevelState"/> for level state management.
/// </summary>
public partial class LevelInitializer : Node2D
{
    public override void _Ready()
    {
        _ = new LevelState();
        LevelState.CurrentLevel = this;

        DrawRoadEdges();
        int count = 0;
        foreach (Node child in GetChildren())
        {
            if (child is RoadEdge edge)
            {
                count++;
                GD.Print($"RoadEdge {count}: A={edge.NodeA.Name} B={edge.NodeB.Name}");
            }
        }
        GD.Print($"Total RoadEdges: {count}");
    }

    private void DrawRoadEdges()
    {
        var intersectionNodes = GetNode("IntersectionNodes").GetChildren();
        if (intersectionNodes == null)
        {
            GD.PrintErr("No RoadNodes found. Check if RoadNodes Node exists.");
            return;
        }

        var roadEdgeScene = GD.Load<PackedScene>(Path.RoadEdgeScene);
        
        foreach (Node node in intersectionNodes)
        {
            if (node is IntersectionNode intersectionNode)
            {
                foreach (IntersectionNode neighbor in intersectionNode.Neighbors)
                {
                    if (intersectionNode.GetInstanceId() < neighbor.GetInstanceId()) // Only draw edge once per pair (avoid duplicates)
                    {
                        var edge = roadEdgeScene.Instantiate<RoadEdge>();
                        AddChild(edge);
                        edge.SetEndpoints(intersectionNode, neighbor);
                    }
                }
            }
        }
    }

}
