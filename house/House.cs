using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class House : Node2D
{
    private Sprite2D _sprite;
    private BusStopDetector _busStopDetector;

    /// <summary>
    /// Indicates whether the house residents can be taken to at least one of
    /// their destinations by at least one route.
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// A convenience property to get the reachable bus stop from the detector
    /// component. Just for readability!
    /// </summary>
    public Node ReachableBusStop => _busStopDetector?.ReachableBusStop;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _busStopDetector = GetNode<BusStopDetector>("BusStopDetector");
        LevelState.AllHouses.Add(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        // GD.Print(ReachableBusStop);
    }


    public void UpdateCheckStatus()
    {
        if (ReachableBusStop == null)
        {
            IsChecked = false;
            return;
        }

        var validDestinationStops = new List<Node>();
        foreach (var destination in LevelState.AllDestinations)
            if (destination.ReachableBusStop != null)
            {
                validDestinationStops.Add(destination.ReachableBusStop);
            }

        bool hasValidRoute = false;
        foreach (var route in LevelState.Routes)
            if (route.PathToTravel.Contains(ReachableBusStop)
            && route.PathToTravel.Any(validDestinationStops.Contains))
            {
                hasValidRoute = true;
                break;
            }

        IsChecked = hasValidRoute;
        if (IsChecked) GD.Print("you won");
    }
}
