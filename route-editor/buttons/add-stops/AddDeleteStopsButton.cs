using Godot;
using System;

public partial class AddDeleteStopsButton : Button
{
    private Node _level;
    private BusStopPlacementArea _busStopPlacementArea;
    public override void _Ready()
    {
        _level = GetTree().CurrentScene as Node ?? GetParent();
        _busStopPlacementArea = _level.GetNode<BusStopPlacementArea>("BusStopPlacementArea");
    }
    private void _on_toggled(bool toggledOn)
    {
        if (toggledOn)
        {
            _busStopPlacementArea.Visible = true;
            EditorState.ActiveTool = EditorTool.AddDeleteBusStop;
        }
        else
        {
            _busStopPlacementArea.Visible = false;
            EditorState.ActiveTool = EditorTool.None;
        }
    }
}
