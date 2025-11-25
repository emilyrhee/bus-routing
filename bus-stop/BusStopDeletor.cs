using Godot;
using System;

public partial class BusStopDeletor : Area2D
{
    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsRightMouseClick())
        {
            return;
        }

        var busStop = GetParent<BusStop>();

        GD.Print("Bus stop deleted: " + busStop.Name);
        busStop.QueueFree();
    }
}
