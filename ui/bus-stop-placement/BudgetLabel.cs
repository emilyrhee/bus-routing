using Godot;
using System;
using static LevelState;

public partial class BudgetLabel : Label
{
    public override void _Ready()
    {
        Text = $"${Budget}";
    }
}
