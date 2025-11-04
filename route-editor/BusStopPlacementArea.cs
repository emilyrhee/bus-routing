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
        Visible = false;
    }

    public override void _Process(double delta)
    {
        if (_previewBusStop != null)
            _previewBusStop.GlobalPosition = GetGlobalMousePosition();
    }

    private void _on_mouse_entered()
    {
        var busStopInstance = _busStopPacked.Instantiate();
        if (busStopInstance is Node2D busStop)
        {
            var level = GetTree().CurrentScene as Node ?? GetParent();
            level.AddChild(busStop);
            busStop.GlobalPosition = GetGlobalMousePosition();

            _previewBusStop = busStop;
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

    private bool IsEventIsLeftMouseClick(InputEvent @event)
    {
        return
            @event is InputEventMouseButton mouseEvent
            && mouseEvent.Pressed
            && mouseEvent.ButtonIndex == MouseButton.Left;
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (IsEventIsLeftMouseClick(@event))
        {
            var busStopInstance = _busStopPacked.Instantiate();

            var level = GetTree().CurrentScene as Node ?? GetParent();

            var previewBusStop = _previewBusStop.GetChild<Area2D>(1);

            if (busStopInstance is Node2D busStop
                && previewBusStop is Area2D previewBusStopArea
                && previewBusStopArea.HasOverlappingAreas())
            {
                level.AddChild(busStop);
                busStop.GlobalPosition = GetGlobalMousePosition();

                _lastPlacedBusStop = busStop;
            }
        }
    }
}
