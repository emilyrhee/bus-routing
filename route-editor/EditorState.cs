using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents all possible states that the route editor can be in.
/// </summary>
public enum EditorTool
{
    None,
    NewRoute
}

/// <summary>
/// Represents the steps involved in creating a new route.
/// </summary>
public enum RouteCreationStep
{
    NotCreating,
    AddingFirstStop,
    AddingSubsequentStops
}

/// <summary>
/// Holds the current state of the route editor and routes.
/// </summary>
public partial class EditorState : Node
{
    /// <summary>
    /// The temporary line segment that follows the cursor during route creation.
    /// </summary>
    public static Line2D RoutePreviewLine { get; set; }

    private static RouteCreationStep? _currentRouteCreationStep = RouteCreationStep.NotCreating;
    public static RouteCreationStep? CurrentRouteCreationStep 
    { 
        get => _currentRouteCreationStep; 
        set => _currentRouteCreationStep = value; 
    }

    private static void RemovePreviewLine()
    {
        RoutePreviewLine?.QueueFree();
        RoutePreviewLine = null;
    }

    private static EditorTool _activeTool = EditorTool.None;
    public static EditorTool ActiveTool
    {
        get => _activeTool;
        set
        {
            _activeTool = value;
            if (_activeTool == EditorTool.NewRoute)
            {
                CurrentRouteCreationStep = RouteCreationStep.AddingFirstStop;
            }
            else
            {
                RemovePreviewLine();
                CurrentRouteCreationStep = null;
            }
        }
    }
}