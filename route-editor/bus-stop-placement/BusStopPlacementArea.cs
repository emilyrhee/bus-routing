using Godot;
using System;
using System.Linq;

public partial class BusStopPlacementArea : Area2D
{
    private PackedScene _busStopScene;
    private PackedScene _roadEdgeScene;

    private Node2D _previewBusStop;
    private Node _currentLevel;
    public override void _Ready()
    {
        _busStopScene = GD.Load<PackedScene>(Path.BusStopScene);
        _roadEdgeScene = GD.Load<PackedScene>(Path.RoadEdgeScene);
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
        var busStopInstance = _busStopScene.Instantiate();
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

    private void SplitEdge(RoadEdge roadEdge, BusStop busStop)
    {
        var edge1 = _roadEdgeScene.Instantiate<RoadEdge>();
        var edge2 = _roadEdgeScene.Instantiate<RoadEdge>();
        _currentLevel.AddChild(edge1);
        _currentLevel.AddChild(edge2);
        edge1.SetEndpoints(roadEdge.NodeA, busStop);
        edge2.SetEndpoints(busStop, roadEdge.NodeB);
        roadEdge.QueueFree();
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsLeftMouseClick())
            return;

        var busStopInstance = _busStopScene.Instantiate();
        var previewBusStop = _previewBusStop.GetChild<Area2D>(1);

        if (busStopInstance is BusStop busStop
            && previewBusStop is Area2D previewBusStopArea
            && previewBusStopArea.HasOverlappingAreas())
        {
            // TODO: Make the bus stop placement exactly on the road edge for cleaner visual
            _currentLevel.AddChild(busStop);
            LevelState.AllBusStops.Add(busStop);
            busStop.GlobalPosition = GetGlobalMousePosition();

            var overlappingArea = previewBusStopArea.GetOverlappingAreas()[0];
            if (overlappingArea is RoadEdge roadEdge
            && roadEdge.NodeA is RoadNode nodeA
            && roadEdge.NodeB is RoadNode nodeB)
            {
                SplitEdge(roadEdge, busStop);

                nodeA.RemoveNeighbor(nodeB);
                nodeB.RemoveNeighbor(nodeA);

                nodeA.AddNeighbor(busStop);
                busStop.AddNeighbor(nodeA);
                nodeB.AddNeighbor(busStop);
                busStop.AddNeighbor(nodeB);

                GD.Print($"NodeA neighbors ({nodeA.Neighbors.Count}): {string.Join(", ", nodeA.Neighbors.Select(n => n.Name))}");
                GD.Print($"NodeB neighbors ({nodeB.Neighbors.Count}): {string.Join(", ", nodeB.Neighbors.Select(n => n.Name))}");
                GD.Print($"BusStop neighbors ({busStop.Neighbors.Count}): {string.Join(", ", busStop.Neighbors.Select(n => n.Name))}");
            }
        }
    }
}
