using Godot;
using System;

public partial class Camera : Camera2D
{
    private bool isDragging = false;
    private Vector2 dragStartPosition;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsMiddleMouseClick())
        {
            isDragging = true;
            dragStartPosition = GetGlobalMousePosition();
        }
        else if (@event.IsMiddleMouseRelease())
        {
            isDragging = false;
        }

        if (@event is InputEventMouseMotion mouseMotionEvent && isDragging)
        {
            Vector2 dragDelta = mouseMotionEvent.Relative;
            
            Position -= dragDelta * Zoom; // Adjust movement speed based on zoom level
        }
    }
}
