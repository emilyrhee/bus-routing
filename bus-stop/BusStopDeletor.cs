using Godot;
using static LevelState;

public partial class BusStopDeletor : Area2D
{
    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsRightMouseClick())
        {
            return;
        }

        var busStop = GetParent<BusStop>();

        foreach (var roadNode in LevelState.AllRoadNodes)
        {
            GD.Print(roadNode.Name + "'s neighbors:");
            foreach (var neighbor in roadNode.Neighbors)
            {
                GD.Print("\t" + neighbor.Name);
            }
        }

        LevelState.AllBusStops.Remove(busStop);
        LevelState.AllRoadNodes.Remove(busStop);

        var A = busStop.Neighbors[0];
        var B = busStop.Neighbors[1];
        A.AddNeighbor(B);
        B.AddNeighbor(A);
        
        A.RemoveNeighbor(busStop);
        B.RemoveNeighbor(busStop);

        busStop.ConnectedEdges.ForEach(edge => edge.QueueFree());

        busStop.QueueFree();

        var roadEdgeScene = GD.Load<PackedScene>(Path.RoadEdgeScene);
        var edge = roadEdgeScene.Instantiate<RoadEdge>();
        CurrentLevel.AddChild(edge);
        edge.SetEndpoints(A, B);
        GD.Print(edge.Name + " created between " + A.GlobalPosition + " and " + B.GlobalPosition);

        foreach (var roadNode in LevelState.AllRoadNodes)
        {
            GD.Print(roadNode.Name + "'s neighbors:");
            foreach (var neighbor in roadNode.Neighbors)
            {
                GD.Print("\t" + neighbor.Name);
            }
        }
    }
}
