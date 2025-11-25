using Godot;
using static EditorState;
using static LineFactory;

public partial class RouteCreationHandler : Area2D
{
    private Node2D _currentLevel;

    public override void _Ready()
    {
        _currentLevel = GetTree().CurrentScene as Node2D ?? GetParent().GetParent() as Node2D;
    }

    public override void _Process(double delta)
    {
        if (RoutePreviewLine == null || CurrentRouteCreationStep != RouteCreationStep.AddingSubsequentStops)
            return;

        if (RoutePreviewLine.GetPointCount() < 2)
            RoutePreviewLine.AddPoint(GetGlobalMousePosition());
        else
            RoutePreviewLine.SetPointPosition(1, GetGlobalMousePosition());
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsLeftMouseClick() || ActiveTool != EditorTool.NewRoute)
        {
            return;
        }

        var clickedRoadNode = GetParent<RoadNode>();

        if (clickedRoadNode is BusStop)
        {
            HandleBusStopClick(clickedRoadNode);
        }
        else
        {
            HandleIntersectionClick(clickedRoadNode);
        }
    }

    private void HandleBusStopClick(RoadNode clickedBusStop)
    {
        var currentRoute = LevelState.Routes[^1];

        switch (CurrentRouteCreationStep)
        {
            case RouteCreationStep.AddingFirstStop:
                currentRoute.AppendNode(clickedBusStop);
                currentRoute.PathVisual = CreateLineAt(clickedBusStop.GlobalPosition);
                _currentLevel.AddChild(currentRoute.PathVisual);

                RoutePreviewLine = CreateLineAt(clickedBusStop.GlobalPosition);
                RoutePreviewLine.DefaultColor = currentRoute.Color;
                _currentLevel.AddChild(RoutePreviewLine);

                CurrentRouteCreationStep = RouteCreationStep.AddingSubsequentStops;
                break;

            case RouteCreationStep.AddingSubsequentStops:
                var lastStop = currentRoute.PathToTravel[^1];
                if (!lastStop.Neighbors.Contains(clickedBusStop))
                {
                    GD.PrintErr("Cannot add stop: Not a neighbor of the previous stop.");
                    return;
                }

                currentRoute.AppendNode(clickedBusStop);

                if (RoutePreviewLine != null)
                {
                    RoutePreviewLine.ClearPoints();
                    RoutePreviewLine.AddPoint(clickedBusStop.GlobalPosition);
                }
                break;
        }
        LevelState.UpdateAllHouseStatuses();
        if (LevelState.IsLevelComplete())
        {
            GD.Print("Level Complete!");
        }
    }

    private void HandleIntersectionClick(RoadNode clickedIntersection)
    {
        if (CurrentRouteCreationStep != RouteCreationStep.AddingSubsequentStops)
        {
            return;
        }

        var currentRoute = LevelState.Routes[^1];
        var lastRoadNode = currentRoute.PathToTravel[^1];

        if (!lastRoadNode.Neighbors.Contains(clickedIntersection))
        {
            GD.PrintErr("Cannot add node: Not a neighbor of the previous stop.");
            return;
        }

        currentRoute.AppendNode(clickedIntersection);

        if (RoutePreviewLine != null)
        {
            RoutePreviewLine.ClearPoints();
            RoutePreviewLine.AddPoint(GlobalPosition);
        }
    }
}