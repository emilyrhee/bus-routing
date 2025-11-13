using Godot;
using System;

public partial class NewRouteButton : Button
{
    private void _on_toggled(bool toggledOn)
    {
        if (toggledOn)
        {
            EditorState.ActiveTool = EditorTool.NewRoute;
            LevelState.Routes.Add(new Route());
        }
        else
        {
            EditorState.ActiveTool = EditorTool.None;
        }
    }
}
