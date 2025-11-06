using Godot;
using System;

public partial class BusStopClickableArea : Area2D
{
    private int test = 0;
    private bool IsNotLeftMouseClick(InputEvent @event)
    {
        return !(@event is InputEventMouseButton mouseEvent
              && mouseEvent.Pressed
              && mouseEvent.ButtonIndex == MouseButton.Left);
    }
    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (IsNotLeftMouseClick(@event))
            return;

        if (RouteEditorState.ActiveTool == EditorState.NewRoute)
        {
            RouteEditorState.Routes[^1].PathToTravel.Add(GetParent());
            foreach (var node in RouteEditorState.Routes[^1].PathToTravel)
            {
                GD.Print("Node in path: " + node);
            }
        }
    }
}