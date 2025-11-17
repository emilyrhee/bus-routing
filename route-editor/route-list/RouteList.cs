using Godot;
using System;

public partial class RouteList : ItemList
{
    private void _on_item_selected(int index)
    {
        GD.Print("Selected route ID: " + LevelState.Routes[index].ID);
    }
}
