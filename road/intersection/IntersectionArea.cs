using Godot;
using static EditorState;

public partial class IntersectionArea : Area2D
{
    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsLeftMouseClick() 
            || ActiveTool != EditorTool.NewRoute 
            || CurrentRouteCreationStep != RouteCreationStep.AddingSubsequentStops)
        {
            return;
        }

        var currentRoute = LevelState.Routes[^1];
        var clickedIntersection = GetParent<RoadNode>();
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
