using Godot;
using System;

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
/// Holds the current state of the route editor. This is autoloaded.
/// </summary>
public partial class RouteEditorState : Node
{
    private EditorState _activeTool = EditorState.None;
    public static EditorState ActiveTool { get; set; }
}