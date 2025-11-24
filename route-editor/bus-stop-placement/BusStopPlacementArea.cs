using Godot;
using System;
using System.Linq;

public partial class BusStopPlacementArea : Area2D
{
    private PackedScene _previewBusStopScene;
    private PackedScene _busStopScene;
    private PackedScene _roadEdgeScene;

    private Node2D _previewBusStop;
    private Node _currentLevel;
    public override void _Ready()
    {
        _previewBusStopScene = GD.Load<PackedScene>(Path.PreviewBusStopScene);
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
        var previewBusStopInstance = _previewBusStopScene.Instantiate();
        if (previewBusStopInstance is Node2D previewBusStop)
        {
            _currentLevel.AddChild(previewBusStop);
            previewBusStop.GlobalPosition = GetGlobalMousePosition();

            _previewBusStop = previewBusStop;
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

    private void CreateBusStopOnEdge(RoadEdge roadEdge, BusStop busStop) // CONSIDER breaking this up further.
    {
        // Calculate the closest point on the road edge to the mouse
        Vector2 p1 = roadEdge.NodeA.GlobalPosition;
        Vector2 p2 = roadEdge.NodeB.GlobalPosition;
        Vector2 mousePosition = GetGlobalMousePosition();
        Vector2 projectedPoint = Geometry2D.GetClosestPointToSegment
        (
            mousePosition, p1, p2
        );

        _currentLevel.AddChild(busStop);
        LevelState.AllBusStops.Add(busStop);
        busStop.GlobalPosition = projectedPoint;

        var nodeA = roadEdge.NodeA;
        var nodeB = roadEdge.NodeB;

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

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsLeftMouseClick())
            return;

        var busStopInstance = _busStopScene.Instantiate();
        var roadPlacementArea = _previewBusStop.GetChild<Area2D>(1);

        if (busStopInstance is BusStop busStop
            && roadPlacementArea.HasOverlappingAreas())
        {
            var overlappingArea = roadPlacementArea.GetOverlappingAreas()[0];
            if (overlappingArea is RoadEdge roadEdge)
            {
                CreateBusStopOnEdge(roadEdge, busStop);
            }
        }
    }
}
