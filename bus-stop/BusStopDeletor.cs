using System.Threading;
using Godot;
using static LevelState;
using System.Linq;
using static Path;

public partial class BusStopDeletor : Area2D
{
    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsRightMouseClick())
        {
            return;
        }

        var busStop = GetParent<BusStop>();

        LevelState.AllBusStops.Remove(busStop);

        foreach (var route in AllRoutes.Where(route => route.ContainsNode(busStop)).ToList())
        {
            route.RemoveNode(busStop);
            if (route.Path.Count <= 1)
            {
                var routeList = GetTree().CurrentScene.GetNode<RouteList>(RouteListNode);
                routeList.DeleteRoute(route);
                LevelState.AllRoutes.Remove(route);
                route.Visual.QueueFree();
                route.QueueFree();
            }
        }

        LevelState.AllRoadNodes.Remove(busStop);
        var A = busStop.Neighbors[0];
        var B = busStop.Neighbors[1];
        A.AddNeighbor(B);
        B.AddNeighbor(A);
        
        A.RemoveNeighbor(busStop);
        B.RemoveNeighbor(busStop);
        
        for (int i = busStop.ConnectedEdges.Count - 1; i >= 0; i--)
        {
            busStop.ConnectedEdges[i]?.QueueFree();
        }

        busStop.QueueFree();

        var roadEdgeScene = GD.Load<PackedScene>(Path.RoadEdgeScene);
        var edge = roadEdgeScene.Instantiate<RoadEdge>();
        CurrentLevel.AddChild(edge);
        edge.SetEndpoints(A, B);
        GD.Print(edge.Name + " created between " + A.GlobalPosition + " and " + B.GlobalPosition);

        UpdateAllHouseStatuses();
    }
}