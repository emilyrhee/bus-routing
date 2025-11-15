using Godot;
using System;

public partial class House : Node2D
{
    private Sprite2D _sprite;
    /// <summary>
    /// Indicates whether the house residents can be taken to at least one of
    /// their destinations by at least one route.
    /// </summary>
    public bool IsChecked { get; set; } = false;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        LevelState.AllHouses.Add(this);
    }
}
