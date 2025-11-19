using Godot;
using System;
using System.ComponentModel.DataAnnotations;

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
        var busStop = (Node2D)_busStopPacked.Instantiate();
        _currentLevel.AddChild(busStop);
        busStop.GlobalPosition = GetGlobalMousePosition();

        _previewBusStop = busStop;
        SetProcess(true);
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

        var busStop = (Node2D)_busStopPacked.Instantiate();
        var previewBusStopArea = _previewBusStop.GetChild<Area2D>(1); // RoadPlacementArea

        if (previewBusStopArea.HasOverlappingAreas())
        {
            //if (valid placement)
            //{
                //GD.Print("valid placement");
                _currentLevel.AddChild(busStop);
                LevelState.AllBusStops.Add(busStop);
                busStop.GlobalPosition = GetGlobalMousePosition();

                _lastPlacedBusStop = busStop;
            //}
            //else
                //GD.Print("invalid placement");
        }
    }
}
