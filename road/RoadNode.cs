using Godot;
using System;
using System.Collections.Generic;

public partial class RoadNode : Node2D
{
    [Export] public Godot.Collections.Array<RoadEdge> IncomingEdges = [];
    [Export] public Godot.Collections.Array<RoadEdge> OutgoingEdges = [];

    public void DispatchBus(Route route)
    {
        var busInstance = GD.Load<PackedScene>("res://bus/Bus.tscn").Instantiate();
        AddChild(busInstance);
    }
}
