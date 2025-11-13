using Godot;
using System;

public partial class AddDeleteStopsButton : Button
{
    private void _on_toggled(bool toggledOn)
    {
        var level = GetTree().CurrentScene as Node ?? GetParent();
        var busStopPlacementArea = level.GetNode<BusStopPlacementArea>("BusStopPlacementArea");

        if (toggledOn)
        {
            busStopPlacementArea.Visible = true;
            EditorState.ActiveTool = EditorTool.AddDeleteBusStop;
        }
        else
        {
            busStopPlacementArea.Visible = false;
            EditorState.ActiveTool = EditorTool.None;
        }
    }
}
