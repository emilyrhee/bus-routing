using Godot;
using static EditorState;
using static LineFactory;
using static LevelState;

public partial class RouteCreationHandler : Area2D
{
    public override void _Process(double delta)
    {
        if (RoutePreviewLine == null || CurrentRouteCreationStep != RouteCreationStep.AddingSubsequentStops)
            return;

        if (RoutePreviewLine.GetPointCount() < 2)
            RoutePreviewLine.AddPoint(GetGlobalMousePosition());
        else
            RoutePreviewLine.SetPointPosition(RoutePreviewLine.GetPointCount() - 1, GetGlobalMousePosition());
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && !mouseButtonEvent.Pressed && CurrentRouteCreationStep == RouteCreationStep.AddingSubsequentStops)
        {
            FinalizeRoute();
        }
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        var clickedRoadNode = GetParent<RoadNode>();

        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.Pressed)
        {
            if (clickedRoadNode is BusStop && CurrentRouteCreationStep == RouteCreationStep.NotCreating)
            {
                StartRouteCreation(clickedRoadNode);
            }
        }
        else if (@event is InputEventMouseMotion && CurrentRouteCreationStep == RouteCreationStep.AddingSubsequentStops)
        {
            GD.Print("Continuing route creation.");
            ContinueRoute(clickedRoadNode);
        }
    }

    private void StartRouteCreation(RoadNode startNode)
    {
        CurrentRouteCreationStep = RouteCreationStep.AddingSubsequentStops;

        var newRoute = new Route();
        LevelState.Routes.Add(newRoute);
        newRoute.AppendNode(startNode);

        newRoute.PathVisual = CreateLineAt(startNode.GlobalPosition);
        CurrentLevel.AddChild(newRoute.PathVisual);

        RoutePreviewLine = CreateLineAt(startNode.GlobalPosition);
        RoutePreviewLine.DefaultColor = newRoute.Color;
        CurrentLevel.AddChild(RoutePreviewLine);
    }

    private void ContinueRoute(RoadNode nextNode)
    {
        var currentRoute = LevelState.Routes[^1];
        var lastNode = currentRoute.PathToTravel[^1];

        if (nextNode != lastNode && lastNode.Neighbors.Contains(nextNode))
        {
            currentRoute.AppendNode(nextNode);
            RoutePreviewLine.SetPointPosition(RoutePreviewLine.GetPointCount() - 1, nextNode.GlobalPosition);
            RoutePreviewLine.AddPoint(nextNode.GlobalPosition);
        }
    }

    private void FinalizeRoute()
    {
        var currentRoute = LevelState.Routes[^1];
        var lastNode = currentRoute.PathToTravel[^1];

        // Check if the route is valid (e.g., ends at a bus stop)
        if (currentRoute.PathToTravel.Count < 2 || !(lastNode is BusStop))
        {
            GD.Print("Route must start and end at a bus stop.");
            currentRoute.PathVisual?.QueueFree();
            LevelState.Routes.Remove(currentRoute);
        }
        else
        {
            // Valid route, update visuals and state
            var routeList = GetTree().CurrentScene.GetNode<ItemList>(Path.RouteListNode);
            routeList.AddItem(currentRoute.ColorName + " line");
            LevelState.UpdateAllHouseStatuses();
            if (LevelState.IsLevelComplete())
            {
                GD.Print("Level Complete!");
            }
        }

        // Cleanup and reset state
        RoutePreviewLine?.QueueFree();
        RoutePreviewLine = null;
        CurrentRouteCreationStep = RouteCreationStep.NotCreating;
        GD.Print("route created: " + currentRoute.PathToTravel.Count + " stops.");
    }
}