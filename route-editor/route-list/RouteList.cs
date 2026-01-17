using Godot;
using System;

public partial class RouteList : ItemList
{
    public override void _UnhandledInput(InputEvent @event)
    {
        // Check if a route is selected and the delete key is pressed
        if (EditorState.SelectedRoute != null && @event.IsActionPressed("ui_text_delete")) // "ui_cancel" is usually the Escape or Delete key
        {
            DeleteRoute(EditorState.SelectedRoute);
            AcceptEvent(); 
        }
    }

    public void DeleteRoute(Route route)
    {
        GD.Print($"Deleting route: {route.ColorName}");

        int itemIndex = LevelState.AllRoutes.IndexOf(route);
        if (itemIndex != -1)
        {
            RemoveItem(itemIndex);
        }

        route.Visual?.QueueFree();
        LevelState.AllRoutes.Remove(route);
        route.QueueFree();

        EditorState.SelectedRoute = null;
        LevelState.UpdateAllHouseStatuses();
    }

    private void _on_item_selected(int index)
    {
        if (index >= 0 && index < LevelState.AllRoutes.Count)
        {
            EditorState.SelectedRoute = LevelState.AllRoutes[index];
            GD.Print($"Selected route for editing: {EditorState.SelectedRoute.ColorName}");
        }
    }

    private new void DeselectAll()
    {
        // Deselect in the UI
        DeselectAll();
        // Clear the selected route from the state
        if (EditorState.SelectedRoute != null)
        {
            GD.Print($"Deselected route: {EditorState.SelectedRoute.ColorName}");
            EditorState.SelectedRoute = null;
        }
    }
}
