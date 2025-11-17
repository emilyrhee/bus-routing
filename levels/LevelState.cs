using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Holds the state of the level, including all routes.
/// Shout out to my friends Mitch, Adam, and Erik for helping me name this.
/// </summary>
public partial class LevelState : Node
{
    public static List<Route> Routes { get; set; } = [];
    public static List<House> AllHouses { get; set; } = [];
    public static List<Destination> AllDestinations { get; set; } = [];
    public static List<Node> AllBusStops { get; set; } = [];
    private static Color[] _routeColors;
    private static int _nextColorIndex = 0;

    static LevelState()
    {
        var colorPalette = GD.Load<Resource>("res://assets/color-palette.tres");
        _routeColors = (Color[])colorPalette.Get("colors");
    }

    public static Color GetNextRouteColor()
    {
        if (_routeColors == null || _routeColors.Length == 0)
        {
            GD.PrintErr("Ran out of colors in the palette.");
            return new Color(1, 1, 1);
        }
        Color color = _routeColors[_nextColorIndex];
        _nextColorIndex = (_nextColorIndex + 1) % _routeColors.Length;
        return color;
    }

    public static bool IsLevelComplete()
    {
        return AllHouses.All(house => house.IsChecked);
    }
    
    public static void UpdateAllHouseStatuses()
    {
        foreach (var house in AllHouses)
        {
            house.UpdateCheckStatus();
        }
    }
}