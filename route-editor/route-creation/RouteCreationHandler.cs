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
            if (SelectedRoute != null && clickedRoadNode is BusStop)
            {
                var selectedRoute = SelectedRoute;
                GD.Print($"Clicked on a bus stop. Selected Route: {selectedRoute.ColorName}, Clicked Node: {clickedRoadNode.Name}");
                if (selectedRoute.PathToTravel.First() == clickedRoadNode || selectedRoute.PathToTravel.Last() == clickedRoadNode)
                {
                    StartRouteEdit(selectedRoute, clickedRoadNode);
                    return;
                }
            }

            if (clickedRoadNode is BusStop && CurrentRouteCreationStep == RouteCreationStep.NotCreating)
            {
                GD.Print("Starting route creation.");
                StartRouteCreation(clickedRoadNode);
            }
        }
        else if (@event is InputEventMouseMotion
        && (CurrentRouteCreationStep == RouteCreationStep.AddingSubsequentStops
        || CurrentRouteCreationStep == RouteCreationStep.EditingRoute))
        {
            ContinueRoute(clickedRoadNode);
        }
    }

    private void StartRouteEdit(Route route, RoadNode startNode)
    {
        GD.Print($"Starting to edit route: {route.ColorName}");
        CurrentRouteCreationStep = RouteCreationStep.EditingRoute;
        _routeBackup = new List<RoadNode>(route.PathToTravel);

        if (route.PathToTravel.First() == startNode)
        {
            IsEditingFromStart = true;
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
        Route routeToEdit = (CurrentRouteCreationStep == RouteCreationStep.EditingRoute) ? SelectedRoute : _tempRoute;
        
        if (routeToEdit == null) return;

        RoadNode lastNode;
        if (IsEditingFromStart)
        {
            GD.Print("Editing from start.");
            lastNode = routeToEdit.PathToTravel.FirstOrDefault();
        }
        else
        {
            GD.Print("Editing from end.");
            lastNode = routeToEdit.PathToTravel.LastOrDefault();
        }
        if (lastNode == null)
        {
            GD.PrintErr("ContinueRoute check failed: lastNode is null.");
            return;
        }

        if (nextNode == lastNode)
        {
            GD.PrintErr("ContinueRoute check failed: nextNode is the same as lastNode.");
            return;
        }

        if (!lastNode.Neighbors.Contains(nextNode))
        {
            GD.PrintErr($"ContinueRoute check failed: '{lastNode.Name}' is not a neighbor of '{nextNode.Name}'.");
            return;
        }

        if (IsEditingFromStart)
        {
            GD.Print($"Continuing route. Prepending '{nextNode.Name}' before '{lastNode.Name}'.");
            routeToEdit.PrependNode(nextNode);
            RoutePreviewLine.SetPointPosition(0, nextNode.GlobalPosition);
            RoutePreviewLine.AddPoint(nextNode.GlobalPosition, 0);
        }
        else
        {
            GD.Print($"Continuing route. Appending '{nextNode.Name}' after '{lastNode.Name}'.");
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
        var editedRoute = SelectedRoute;
        var firstNode = editedRoute.PathToTravel.First();
        var lastNode = editedRoute.PathToTravel.Last();
        GD.Print("Final path: " + string.Join(" -> ", editedRoute.PathToTravel.Select(n => n.Name)));

        if (editedRoute.PathToTravel.Count < 2 || firstNode is not BusStop || lastNode is not BusStop)
        {
            GD.PrintErr("Edited route is invalid. Reverting.");
            GD.Print("Reverting to: " + string.Join(" -> ", _routeBackup.Select(n => n.Name)));
            editedRoute.SetPath(_routeBackup); // Revert to backup
        }
        else
        {
            GD.Print("Route edit successful.");
            UpdateAllHouseStatuses();
        }
        _routeBackup = null;
        ResetState();
    }

    private void ResetState()
    {
        RoutePreviewLine?.QueueFree();
        RoutePreviewLine = null;
        CurrentRouteCreationStep = RouteCreationStep.NotCreating;
        IsEditingFromStart = false;
    }
}