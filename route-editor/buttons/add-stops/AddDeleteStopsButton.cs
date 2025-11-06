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
            RouteEditorState.ActiveTool = EditorState.AddDeleteBusStop;
        }
        else
        {
            busStopPlacementArea.Visible = false;
            RouteEditorState.ActiveTool = EditorState.None;
        }
    }
}
