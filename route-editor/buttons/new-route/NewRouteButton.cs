using Godot;
using System;

public partial class NewRouteButton : Button
{
    private Node _level;
    private ItemList _routeList;
    public override void _Ready()
    {
        _level = GetTree().CurrentScene as Node ?? GetParent();
        _routeList = _level.GetNode<ItemList>("EditorUI/RouteList/RouteList");
    }

    private void _on_toggled(bool toggledOn)
    {
        if (toggledOn)
        {
            EditorState.ActiveTool = EditorTool.NewRoute;
            var newRoute = new Route();
            LevelState.Routes.Add(newRoute);
            _routeList.AddItem(newRoute.ColorName + " line");
        }
        else
        {
            EditorState.ActiveTool = EditorTool.None;
        }
    }
}
