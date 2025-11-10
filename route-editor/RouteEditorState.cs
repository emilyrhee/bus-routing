using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Holds all possible states that the route editor can be in.
/// </summary>
public enum EditorTool
{
    None,
    AddDeleteBusStop,
    NewRoute
}

/// <summary>
/// Holds the current state of the route editor and routes.
/// </summary>
public partial class RouteEditorState : Node
{
    private EditorTool _activeTool = EditorTool.None;
    public static EditorTool ActiveTool { get; set; }
}