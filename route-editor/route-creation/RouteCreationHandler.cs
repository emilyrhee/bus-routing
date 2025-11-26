using Godot;
using static EditorState;
using static LineFactory;
using static LevelState;

public partial class RouteCreationHandler : Area2D
{
    /// <summary>
    /// This exists so that we do not need to add a Route to LevelState until
    /// it's fully created and valid.
    /// </summary>
    private static Route _tempRoute;

    public override void _Process(double delta)
    {
        if (RoutePreviewLine == null
        || CurrentRouteCreationStep != RouteCreationStep.AddingSubsequentStops)
            return;

        if (RoutePreviewLine.GetPointCount() < 2)
            RoutePreviewLine.AddPoint(GetGlobalMousePosition());
        else
            RoutePreviewLine.SetPointPosition
            (
                RoutePreviewLine.GetPointCount() - 1, GetGlobalMousePosition()
            );
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsLeftMouseRelease()
        && CurrentRouteCreationStep == RouteCreationStep.AddingSubsequentStops)
        {
            FinalizeRoute();
        }
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        var clickedRoadNode = GetParent<RoadNode>();

        if (@event.IsLeftMouseClick())
        {
            if (clickedRoadNode is BusStop && CurrentRouteCreationStep == RouteCreationStep.NotCreating)
            {
                GD.Print("Starting route creation.");
                StartRouteCreation(clickedRoadNode);
            }
        }
        else if (@event is InputEventMouseMotion && CurrentRouteCreationStep == RouteCreationStep.AddingSubsequentStops)
        {
            ContinueRoute(clickedRoadNode);
        }
    }

    private void StartRouteCreation(RoadNode startNode)
    {
        GD.Print("Route creation started at: " + startNode.Name);
        CurrentRouteCreationStep = RouteCreationStep.AddingSubsequentStops;

        _tempRoute = new Route();
        _tempRoute.AppendNode(startNode);

        _tempRoute.PathVisual = CreateLineAt(startNode.GlobalPosition);
        CurrentLevel.AddChild(_tempRoute.PathVisual);

        RoutePreviewLine = CreateLineAt(startNode.GlobalPosition);
        RoutePreviewLine.DefaultColor = _tempRoute.Color;
        CurrentLevel.AddChild(RoutePreviewLine);
    }

    private void ContinueRoute(RoadNode nextNode)
    {
        var lastNode = _tempRoute.PathToTravel[^1];

        if (nextNode != lastNode && lastNode.Neighbors.Contains(nextNode))
        {
            _tempRoute.AppendNode(nextNode);
            RoutePreviewLine.SetPointPosition(RoutePreviewLine.GetPointCount() - 1, nextNode.GlobalPosition);
            RoutePreviewLine.AddPoint(nextNode.GlobalPosition);
        }
    }

    private void FinalizeRoute()
    {
        var lastNode = _tempRoute.PathToTravel[^1];

        // Check if the route is valid
        if (_tempRoute.PathToTravel.Count < 2 || lastNode is not BusStop)
        {
            GD.PrintErr("Route must start and end at a bus stop.");
            _tempRoute.PathVisual?.QueueFree();
            LevelState.ReturnLastRouteColor();
        }
        else
        {
            // Valid route, add to LevelState and update visuals
            LevelState.Routes.Add(_tempRoute);
            var routeList = GetTree().CurrentScene.GetNode<ItemList>(Path.RouteListNode);
            routeList.AddItem(_tempRoute.ColorName + " line");
            LevelState.UpdateAllHouseStatuses();
            if (LevelState.IsLevelComplete())
            {
                GD.Print("Level Complete!");
            }
        }

        // Cleanup and reset state
        RoutePreviewLine?.QueueFree();
        RoutePreviewLine = null;
        _tempRoute = null;
        CurrentRouteCreationStep = RouteCreationStep.NotCreating;
        GD.Print("Current routes in level: " + LevelState.Routes.Count);
        foreach (var route in LevelState.Routes)
        {
            GD.Print($"Route: {route.ColorName}"); 
            foreach (var roadNode in route.PathToTravel)
                GD.Print($"  PathToTravel: {roadNode.Name}");
        }
    }
}