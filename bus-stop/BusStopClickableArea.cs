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
        currentRoute.PathToTravel.Add(GetParent());

        var clickedBusStopPosition = ((Node2D)GetParent()).GlobalPosition;

        switch (CurrentRouteCreationStep)
        {
            case RouteCreationStep.AddingFirstStop:
                currentRoute.PathVisual = CreateLineAt(clickedBusStopPosition);
                _currentLevel.AddChild(currentRoute.PathVisual);

                RoutePreviewLine = CreateLineAt(clickedBusStopPosition);
                RoutePreviewLine.DefaultColor = currentRoute.Color;
                _currentLevel.AddChild(RoutePreviewLine);

                CurrentRouteCreationStep = RouteCreationStep.AddingSubsequentStops;
                break;
            case RouteCreationStep.AddingSubsequentStops:
                currentRoute.PathVisual.AddPoint(clickedBusStopPosition);

                // Repositions temporary preview line to start from this new stop.
                RoutePreviewLine.ClearPoints();
                RoutePreviewLine.AddPoint(clickedBusStopPosition);
                break;
        }
        LevelState.UpdateAllHouseStatuses();
    }
}