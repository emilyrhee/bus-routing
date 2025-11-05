using Godot;
using System;

public partial class AddDeleteStopsButton : Button
{
    private void _on_toggled(bool toggledOn)
    {
        var level = GetTree().CurrentScene as Node ?? GetParent();
        var busStopPlacementArea = level.GetNode<BusStopPlacementArea>("BusStopPlacementArea");
        var routeCreationArea = level.GetNode<RouteCreationArea>("RouteCreationArea");

        if (toggledOn)
        {
            busStopPlacementArea.Visible = true;
            routeCreationArea.Visible = false;
        }
        else
        {
            busStopPlacementArea.Visible = false;
        }
    }
}
