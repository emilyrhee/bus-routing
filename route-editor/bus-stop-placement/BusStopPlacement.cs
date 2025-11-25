using Godot;
using System;
using System.Linq;
using static LevelState;

public partial class BusStopPlacement : Control
{
    private PackedScene _previewBusStopScene;
    private PackedScene _busStopScene;
    private PackedScene _roadEdgeScene;

    private PreviewBusStop _previewBusStop;
    private Area2D _previewPlacementArea;
    private bool _isValidPlacement = false;

    public override void _Ready()
    {
        _previewBusStopScene = GD.Load<PackedScene>(Path.PreviewBusStopScene);
        _busStopScene = GD.Load<PackedScene>(Path.BusStopScene);
        _roadEdgeScene = GD.Load<PackedScene>(Path.RoadEdgeScene);
        MouseFilter = MouseFilterEnum.Pass; // Allow mouse events to pass through to underlying nodes
    }

    public override void _Process(double delta)
    {
        if (_previewBusStop != null)
        {
            _previewBusStop.GlobalPosition = GetGlobalMousePosition();
            _isValidPlacement = _previewPlacementArea.HasOverlappingAreas()
            && !_previewBusStop.IntersectionDetector.HasOverlappingAreas();

            var color = _isValidPlacement ? Colors.LightGreen : Colors.Red;
            _previewBusStop.Modulate = color;
        }
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data.AsString() == "BusStop")
        {
            if (_previewBusStop == null)
            {
                _previewBusStop = _previewBusStopScene.Instantiate<PreviewBusStop>();
                CurrentLevel.AddChild(_previewBusStop);
                _previewPlacementArea = _previewBusStop.GetChild<Area2D>(1);
                SetProcess(true);
            }
            return true;
        }
        return false;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        if (_isValidPlacement)
        {
            var busStopInstance = _busStopScene.Instantiate<BusStop>();
            var overlappingArea = _previewPlacementArea.GetOverlappingAreas().FirstOrDefault();
            if (overlappingArea is RoadEdge roadEdge)
            {
                CreateBusStopOnEdge(roadEdge, busStopInstance);
            }
        }
        CleanupPreview();
    }
    
    private void CleanupPreview()
    {
        if (_previewBusStop != null)
        {
            _previewBusStop.QueueFree();
            _previewBusStop = null;
            _previewPlacementArea = null;
            _isValidPlacement = false;
            SetProcess(false);
        }
    }

    private void SplitEdge(RoadEdge roadEdge, BusStop busStop)
    {
        var edge1 = _roadEdgeScene.Instantiate<RoadEdge>();
        var edge2 = _roadEdgeScene.Instantiate<RoadEdge>();
        CurrentLevel.AddChild(edge1);
        CurrentLevel.AddChild(edge2);
        edge1.SetEndpoints(roadEdge.NodeA, busStop);
        edge2.SetEndpoints(busStop, roadEdge.NodeB);
        roadEdge.QueueFree();
    }

    private void CreateBusStopOnEdge(RoadEdge roadEdge, BusStop busStop)
    {
        Vector2 p1 = roadEdge.NodeA.GlobalPosition;
        Vector2 p2 = roadEdge.NodeB.GlobalPosition;
        Vector2 mousePosition = GetGlobalMousePosition();
        Vector2 projectedPoint = Geometry2D.GetClosestPointToSegment(mousePosition, p1, p2);

        CurrentLevel.AddChild(busStop);
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
    }
}
