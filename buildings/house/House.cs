using Godot;
using System.Linq;

public partial class House : Building
{
    private Sprite2D _checkSprite;
    private bool _isChecked;

    protected override Color HighlightFactor => new(1.4f, 1.4f, 1.4f, 1.0f);

    public bool IsChecked
    {
        get => _isChecked;
        private set
        {
            _isChecked = value;
            _checkSprite.Visible = value;
        }
    }

    public override void _Ready()
    {
        base._Ready(); // Calls _Ready() of the base class, Building.
        _checkSprite = GetNode<Sprite2D>("Check");
        LevelState.AllHouses.Add(this);
    }

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
