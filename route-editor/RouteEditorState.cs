using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Holds all possible states that the route editor can be in.
/// </summary>
public enum EditorState
{
    None,
    AddDeleteBusStop,
    NewRoute
}

/// <summary>
/// Holds the current state of the route editor and routes. This is autoloaded.
/// </summary>
public partial class RouteEditorState : Node
{
    private EditorState _activeTool = EditorState.None;
    public static EditorState ActiveTool { get; set; }

    public static List<Route> Routes { get; set; } = [];
}