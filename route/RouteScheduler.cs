using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class RouteScheduler : Node
{
    private GlobalTime _globalTime;
    private List<Route> _activeRoutes = [];

    public override void _Ready()
    {
        _globalTime = GetNode<GlobalTime>("/root/GlobalTime");
        _globalTime.TimeAdvanced += OnGlobalTimeAdvanced;
        Route testRoute = new Route(new Time(5), 10);
        testRoute.Stops = new Godot.Collections.Array<RoadNode>()
        {
            
        };

        _activeRoutes.Add(testRoute);
        GD.Print("RouteScheduler is ready and listening for time updates.");
    }

    private void OnGlobalTimeAdvanced()
    {
        foreach (var route in _activeRoutes)
        {
            GD.Print("route starts at: " + route.StartTime.GetFormattedTimeString() + ". current time: " + _globalTime.GameTime.GetFormattedTimeString());
            if (route.StartTime.Equals(_globalTime.GameTime))
            {
                // TODO: Make bus dispatch to the first stop in the route's stop list
                GetNode<RoadNode>("/root/Meriden/BusDepotNode").DispatchBus();
                GD.Print($"Dispatching bus for route starting at {route.StartTime.GetFormattedTimeString()}");
            }
        }
    }
}
