using Godot;
using System;

public partial class BusStopPlacementArea : Area2D
{
    private PackedScene _busStopPacked;
    private Node2D _lastPlacedBusStop;

    private Node2D _previewBusStop;
    public override void _Ready()
    {
        _busStopPacked = GD.Load<PackedScene>("res://bus-stop/bus-stop.tscn");
        if (_busStopPacked == null)
            GD.PushError("Failed to load bus-stop.tscn in BusStopPlacementArea._Ready()");
    }

    public override void _Process(double delta)
    {
        if (_previewBusStop != null)
            _previewBusStop.GlobalPosition = GetGlobalMousePosition();
    }

    private void _on_mouse_entered()
    {
        var busStopInstance = _busStopPacked.Instantiate();
        if (busStopInstance is Node2D node)
        {
            var parent = GetTree().CurrentScene as Node ?? GetParent();
            parent.AddChild(node);
            node.GlobalPosition = GetGlobalMousePosition();

            _previewBusStop = node;
            SetProcess(true);
        }
    }

    private void _on_mouse_exited()
    {
        if (_previewBusStop != null)
        {
            _previewBusStop.QueueFree();
            _previewBusStop = null;
            SetProcess(false);
        }        
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent
            && mouseEvent.Pressed
            && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (_busStopPacked == null)
                return;

            var busStopInstance = _busStopPacked.Instantiate();

            var parent = GetTree().CurrentScene as Node ?? GetParent();

            if (busStopInstance is Node2D busStop) // this condition is needed because Nodes do not have positions, but Node2Ds do.
            {
                parent.AddChild(busStop);
                busStop.GlobalPosition = GetGlobalMousePosition();

                _lastPlacedBusStop = busStop;
            }
        }
    }
}
