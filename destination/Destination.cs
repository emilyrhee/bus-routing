using Godot;
using System;

public partial class Destination : Node2D
{
    private BusStopDetector _busStopDetector;

    /// <summary>
    /// A convenience property to get the reachable bus stop from the detector
    /// component. Just for readability!
    /// </summary>
    public Node ReachableBusStop => _busStopDetector?.ReachableBusStop;
    public override void _Ready()
    {
        _busStopDetector = GetNode<BusStopDetector>("BusStopDetector");
        LevelState.AllDestinations.Add(this);
    }
}
