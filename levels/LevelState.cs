using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Holds the state of the level, including all routes.
/// Shout out to my friends Mitch, Adam, and Erik for helping me name this.
/// </summary>
public partial class LevelState : Node
{
    public static List<Route> Routes { get; set; } = [];
    public static List<House> AllHouses { get; set; } = [];
}