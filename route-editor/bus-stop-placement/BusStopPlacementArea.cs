using Godot;
using System;

public partial class BusStopPlacementArea : Area2D
{
    private PackedScene _busStopPacked;
    private Node2D _lastPlacedBusStop;

    private Node2D _previewBusStop;
    private Node _currentLevel;
    public override void _Ready()
    {
        _busStopPacked = GD.Load<PackedScene>("res://bus-stop/bus-stop.tscn");
        _currentLevel = GetTree().CurrentScene as Node ?? GetParent();
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
            _currentLevel.AddChild(busStop);
            busStop.GlobalPosition = GetGlobalMousePosition();

            _previewBusStop = busStop;
            SetProcess(true);
        }
    }

    private void _on_mouse_exited()
    {
        _previewBusStop.QueueFree();
        SetProcess(false);
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsLeftMouseClick())
            return;

        var busStopInstance = _busStopPacked.Instantiate();
        var previewBusStop = _previewBusStop.GetChild<Area2D>(1);

        if (busStopInstance is Node2D busStop
            && previewBusStop is Area2D previewBusStopArea
            && previewBusStopArea.HasOverlappingAreas())
        {
            _currentLevel.AddChild(busStop);
            busStop.GlobalPosition = GetGlobalMousePosition();

            _lastPlacedBusStop = busStop;
        }
    }
}
