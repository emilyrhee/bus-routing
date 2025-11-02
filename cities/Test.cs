using Godot;
using System;

public partial class Test : CollisionShape2D
{
public override void _Input(InputEvent @event)
{
    if (@event is InputEventMouseButton mouseEvent)
    {
        GD.Print("mouse button event at ", mouseEvent.Position);
    }
}
}
