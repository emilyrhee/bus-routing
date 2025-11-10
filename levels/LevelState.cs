using Godot;
using System;
using System.Collections.Generic;

public partial class LevelState : Node
{
    public static List<Route> Routes { get; set; } = []; // should go in "app data"? or something like the app data, you know what I'm sayin? thats right.
}