using Godot;
using System;

public partial class House : Node2D
{
    int inhabitantsAtHome = 1;
    public override void _Ready()
    {
        var car = GD.Load<PackedScene>("res://vehicles/car/car.tscn");
        var carInstance = car.Instantiate();
        AddChild(carInstance);
    }
}
