using Godot;
using System;
using System.Linq;

public partial class House : Node2D
{
    private BusStopDetector _busStopDetector;
    private Sprite2D _checkSprite;

    private bool _isChecked;
    /// <summary>
    /// Indicates whether the house residents can be taken to at least one of
    /// their destinations by at least one route.
    /// The setter automatically updates the checkmark sprite's visibility.
    /// </summary>
    public bool IsChecked
    {
        get => _isChecked;
        private set
        {
            _isChecked = value;
            _checkSprite.Visible = value;
        }
    }

    /// <summary>
    /// A convenience property to get the reachable bus stop from the detector
    /// component. Just for readability!
    /// </summary>
    public Node ReachableBusStop => _busStopDetector?.ReachableBusStop;

    public override void _Ready()
    {
        _busStopDetector = GetNode<BusStopDetector>("BusStopDetector");
        _checkSprite = GetNode<Sprite2D>("Check");
        LevelState.AllHouses.Add(this);
    }

    /// <summary>
    /// Handles whether or not a house should be checked.
    /// Modifies IsChecked property.
    /// </summary>
    public void UpdateCheckStatus()
    {
        if (ReachableBusStop == null)
        {
            IsChecked = false;
            return;
        }

        var validDestinationStops = LevelState.AllDestinations
            .Where(destination => destination.Modulate == Modulate
            && destination.ReachableBusStop != null)
            .Select(destination => destination.ReachableBusStop)
            .ToHashSet();

        if (validDestinationStops.Count == 0)
        {
            IsChecked = false;
            return;
        }

        bool isSatisfied = LevelState.AllRoutes.Any(route =>
            route.Path.Contains(ReachableBusStop) &&
            route.Path.Any(validDestinationStops.Contains)
        );

        IsChecked = isSatisfied;
    }
}
