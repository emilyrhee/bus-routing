using Godot;
using System;

public partial class AddStops : Control
{
    private void _on_button_toggled(bool buttonPressed)
    {
        var level = GetTree().CurrentScene as Node ?? GetParent();
        var busStopPlacementArea = level.GetNode<BusStopPlacementArea>("BusStopPlacementArea");

        if (buttonPressed)
        {
            busStopPlacementArea.Visible = true;
        }
        else
        {
            busStopPlacementArea.Visible = false;
        }
    }
}