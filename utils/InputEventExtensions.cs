using Godot;
using System;

/// <summary>
/// Extension methods for Godot's InputEvent.
/// </summary>
public static class InputEventExtensions
{
    public static bool IsLeftMouseClick(this InputEvent @event)
    {
        return @event is InputEventMouseButton mouseEvent
              && mouseEvent.Pressed
              && mouseEvent.ButtonIndex == MouseButton.Left;
    }
}