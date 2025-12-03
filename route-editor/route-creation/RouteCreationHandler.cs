using Godot;
using System.Collections.Generic;
using System.Linq;
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
    private static List<RoadNode> _routeBackup; // For reverting invalid edits

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
        && (CurrentRouteCreationStep == RouteCreationStep.AddingSubsequentStops || CurrentRouteCreationStep == RouteCreationStep.EditingRoute))
        {
            FinalizeRoute();
        }
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        var clickedRoadNode = GetParent<RoadNode>();

        if (@event.IsLeftMouseClick())
        {
            if (EditorState.SelectedRoute != null && clickedRoadNode is BusStop)
            {
                var selectedRoute = EditorState.SelectedRoute;
                if (selectedRoute.PathToTravel.First() == clickedRoadNode || selectedRoute.PathToTravel.Last() == clickedRoadNode)
                {
                    StartRouteEdit(selectedRoute, clickedRoadNode);
                    return; // Stop further processing
                }
            }

            if (clickedRoadNode is BusStop && CurrentRouteCreationStep == RouteCreationStep.NotCreating)
            {
                GD.Print("Starting route creation.");
                StartRouteCreation(clickedRoadNode);
            }
        }
        else if (@event is InputEventMouseMotion && (CurrentRouteCreationStep == RouteCreationStep.AddingSubsequentStops || CurrentRouteCreationStep == RouteCreationStep.EditingRoute))
        {
            ContinueRoute(clickedRoadNode);
        }
    }

    private void StartRouteEdit(Route route, RoadNode startNode)
    {
        GD.Print($"Starting to edit route: {route.ColorName}");
        CurrentRouteCreationStep = RouteCreationStep.EditingRoute;

        // Back up the current path in case of invalid edit
        _routeBackup = new List<RoadNode>(route.PathToTravel);

        // If starting from the beginning of the route, reverse it first
        if (route.PathToTravel.First() == startNode)
        {
            route.PathToTravel.Reverse();
            route.SetPath(route.PathToTravel); // Redraw visuals
        }

        // Setup the preview line
        RoutePreviewLine = CreateLineAt(startNode.GlobalPosition);
        RoutePreviewLine.DefaultColor = route.Color;
        CurrentLevel.AddChild(RoutePreviewLine);
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
        Route routeToEdit = (CurrentRouteCreationStep == RouteCreationStep.EditingRoute) ? EditorState.SelectedRoute : _tempRoute;
        
        if (routeToEdit == null) return;

        var lastNode = routeToEdit.PathToTravel.LastOrDefault();

        if (lastNode != null && nextNode != lastNode && lastNode.Neighbors.Contains(nextNode))
        {
            routeToEdit.AppendNode(nextNode);
            RoutePreviewLine.SetPointPosition(RoutePreviewLine.GetPointCount() - 1, nextNode.GlobalPosition);
            RoutePreviewLine.AddPoint(nextNode.GlobalPosition);
        }
    }

    private void FinalizeRoute()
    {
        if (CurrentRouteCreationStep == RouteCreationStep.EditingRoute)
        {
            FinalizeRouteEdit();
        }
        else if (CurrentRouteCreationStep == RouteCreationStep.AddingSubsequentStops)
        {
            FinalizeRouteCreation();
        }
    }

    private void FinalizeRouteCreation()
    {
        var lastNode = _tempRoute.PathToTravel[^1];
        if (_tempRoute.PathToTravel.Count < 2 || lastNode is not BusStop)
        {
            GD.PrintErr("Route must start and end at a bus stop.");
            _tempRoute.PathVisual?.QueueFree();
            LevelState.ReturnLastRouteColor();
        }
        else
        {
            LevelState.Routes.Add(_tempRoute);
            var routeList = GetTree().CurrentScene.GetNode<ItemList>(Path.RouteListNode);
            routeList.AddItem(_tempRoute.ColorName + " line");
            LevelState.UpdateAllHouseStatuses();
        }
        _tempRoute = null;
        ResetState();
    }

    private void FinalizeRouteEdit()
    {
        var editedRoute = EditorState.SelectedRoute;
        var lastNode = editedRoute.PathToTravel.Last();

        if (editedRoute.PathToTravel.Count < 2 || lastNode is not BusStop)
        {
            GD.PrintErr("Edited route is invalid. Reverting.");
            editedRoute.SetPath(_routeBackup); // Revert to backup
        }
        else
        {
            GD.Print("Route edit successful.");
            LevelState.UpdateAllHouseStatuses();
        }
        _routeBackup = null;
        ResetState();
    }

    private void ResetState()
    {
        RoutePreviewLine?.QueueFree();
        RoutePreviewLine = null;
        CurrentRouteCreationStep = RouteCreationStep.NotCreating;
    }
}