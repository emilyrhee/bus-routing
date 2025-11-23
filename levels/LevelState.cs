using Godot;
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
    public static List<RoadNode> AllRoadNodes { get; set; } = [];

    private static int _nextColorIndex = 0;

    public static KeyValuePair<string, Color>? GetNextRouteColor()
    {
        if (_nextColorIndex >= RouteColors.ColorList.Count)
        {
            GD.PrintErr("All available route colors have been used.");
            return null;
        }
        var colorInfo = RouteColors.ColorList[_nextColorIndex];
        _nextColorIndex++;
        return colorInfo;
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