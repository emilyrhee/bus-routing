using Godot;
using System;

public partial class BusStopPlacementArea : Area2D
{
    public override void _Ready()
    {

    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent
            && mouseEvent.Pressed
            && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            var globalPosition = GetGlobalMousePosition();

            var packed = GD.Load<PackedScene>("res://bus-stop/bus-stop.tscn");
            var busStopInstance = packed.Instantiate();

            var parent = GetTree().CurrentScene as Node ?? GetParent();

            if (busStopInstance is Node2D busStop) // this condition is needed because Nodes do not have positions, but Node2Ds do.
            {
                parent.AddChild(busStop);
                busStop.GlobalPosition = globalPosition;
            }
        }
    }
}
