using Godot;
using System;

public partial class NewRouteButton : Button
{
    private void _on_toggled(bool toggledOn)
    {
        if (toggledOn)
        {
            RouteEditorState.ActiveTool = EditorState.NewRoute;
            RouteEditorState.Routes.Add(new Route());
        }
        else
        {
            RouteEditorState.ActiveTool = EditorState.None;
        }
    }
}
