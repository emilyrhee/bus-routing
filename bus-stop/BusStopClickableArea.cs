using Godot;
using System;

public partial class BusStopClickableArea : Area2D
{
    private int test = 0;
    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsLeftMouseClick())
            return;

        if (RouteEditorState.ActiveTool == EditorState.NewRoute)
        {
            RouteEditorState.Routes[^1].PathToTravel.Add(GetParent());
            GD.Print("Added bus stop to route. Current path:");
            foreach (var node in RouteEditorState.Routes[^1].PathToTravel)
            {
                GD.Print(node);
            }
        }
    }
}