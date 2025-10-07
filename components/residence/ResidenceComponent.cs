using Godot;
using System;

public partial class ResidenceComponent : Node2D
{
    public override void _Ready()
    {
        var car = GD.Load<PackedScene>("res://vehicle/vehicle.tscn");
        var carInstance = car.Instantiate();
        AddChild(carInstance);
    }
}