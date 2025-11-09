using Godot;
using System;

public static class InputEventExtensions
{
    public static bool IsLeftMouseClick(this InputEvent @event)
    {
        return @event is InputEventMouseButton mouseEvent
              && mouseEvent.Pressed
              && mouseEvent.ButtonIndex == MouseButton.Left;
    }
}