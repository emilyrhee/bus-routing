using Godot;
using System;

public partial class NewRouteButton : Button
{
    private void _on_toggled(bool toggledOn)
    {
        var level = GetTree().CurrentScene as Node ?? GetParent();
        var routeCreationArea = level.GetNode<RouteCreationArea>("RouteCreationArea");

        if (toggledOn)
        {
            routeCreationArea.Visible = true;
        }
        else
        {
            routeCreationArea.Visible = false;
        }
    }
}
