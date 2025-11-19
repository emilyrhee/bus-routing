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
    private static int _nextColorIndex = 0;
    public RoadNetwork RoadNetwork { get; private set; }

    public override void _Ready()
    {
        var roadNodeContainer = GetNodeOrNull<Node>("/root/Meriden/Roads/Nodes");
        if (roadNodeContainer == null)
        {
            GD.PrintErr("Could not find the 'Roads/Nodes' container. Please check the node path.");
            return;
        }

        RoadNetwork = new RoadNetwork(roadNodeContainer);
        RoadNetwork.PrintAdjacencyMatrix();
    }

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