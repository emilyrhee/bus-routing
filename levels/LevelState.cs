using Godot;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Holds the state of the level, including all routes.
/// Shout out to my friends Mitch, Adam, and Erik for helping me name this.
/// </summary>
public partial class LevelState : Node
{
    public static Node CurrentLevel { get; set; }
    public static List<Route> AllRoutes { get; set; } = [];
    public static List<House> AllHouses { get; set; } = [];
    public static List<Destination> AllDestinations { get; set; } = [];
    public static List<Node> AllBusStops { get; set; } = [];
    public static List<RoadNode> AllRoadNodes { get; set; } = [];
    public static List<RoadEdge> AllRoadEdges { get; set; } = [];

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

    /// <summary>
    /// Returns the last used route color back to the pool.
    /// Needed when a route creation is cancelled e.g. only one stop added or
    /// doesn't end at a bus stop.
    /// 
    /// Is this the best way to handle this? Probably not. But it works for now.
    /// </summary>
    public static void ReturnLastRouteColor()
    {
        if (_nextColorIndex > 0)
        {
            _nextColorIndex--;
        }
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