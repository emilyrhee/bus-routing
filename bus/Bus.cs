using Godot;
using System;

public partial class Bus : Node2D
{
    private Route _route;
    public Route Route
    {
        get => _route;
        set => _route = value;
    }
    public override void _Ready()
    {
        MoveAlongPath();
    }
    public void MoveAlongPath()
    {
        // Bus movement logic here
    }
}