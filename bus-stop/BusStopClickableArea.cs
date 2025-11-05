using Godot;
using System;

public partial class BusStopClickableArea : Area2D
{
    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent
            && mouseEvent.Pressed
            && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            GD.Print("Bus stop clicked!");
        }
    }
}