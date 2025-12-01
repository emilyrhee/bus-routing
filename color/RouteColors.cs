using Godot;
using System.Collections.Generic;

/// <summary>
/// A collection of key-value pairs to associate color names with their
/// values.
/// </summary>
public static class RouteColors
{
    public static readonly List<KeyValuePair<string, Color>> ColorList =
    [
        new("Orange", new Color("E28554", 0.8f)),
        new("Blue", new Color("486AF5", 0.8f)),
        new("Green", new Color("18905C", 0.8f)),
        new("Purple", new Color("A958FF", 0.8f))
    ];
}