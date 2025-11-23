using Godot;
using static LineFactory;
using static EditorState;

public partial class BusStopClickableArea : Area2D
{
    private Node2D _currentLevel;
    public override void _Ready()
    {
        _currentLevel = GetTree().CurrentScene as Node2D ?? GetParent() as Node2D;
    }

    public override void _Process(double delta)
    {
        if (RoutePreviewLine == null)
            return;

        if (RoutePreviewLine.GetPointCount() < 2)
            RoutePreviewLine.AddPoint(GetGlobalMousePosition());
        else
            RoutePreviewLine.SetPointPosition(1, GetGlobalMousePosition());
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsLeftMouseClick()
            || ActiveTool != EditorTool.NewRoute)
            return;

        var currentRoute = LevelState.Routes[^1];
        var clickedBusStop = GetParent<RoadNode>();

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
                    GD.PrintErr
                    (
                        "Cannot add stop: Not a neighbor of the previous stop."
                    );
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
    }
}