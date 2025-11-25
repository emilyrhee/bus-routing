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
        currentRoute.AppendNode(GetParent<RoadNode>());

        if (RoutePreviewLine != null)
        {
            RoutePreviewLine.ClearPoints();
            RoutePreviewLine.AddPoint(GlobalPosition);
        }
    }
}
