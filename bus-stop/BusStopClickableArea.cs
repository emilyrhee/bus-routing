using Godot;
using System;

public partial class BusStopClickableArea : Area2D
{
    private int test = 0;
    private Node2D _currentLevel;
    private Line2D _previewLine;

    public override void _Ready()
    {
        _currentLevel = GetTree().CurrentScene as Node2D ?? GetParent() as Node2D;
    }

    public override void _Process(double delta)
    {
        if (_previewLine == null)
            return;

        if (_previewLine.GetPointCount() < 2)
            _previewLine.AddPoint(GetGlobalMousePosition());
        else
            _previewLine.SetPointPosition(1, GetGlobalMousePosition());
    }

    private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!@event.IsLeftMouseClick()
            || RouteEditorState.ActiveTool != EditorTool.NewRoute)
            return;

        LevelState.Routes[^1].PathToTravel.Add(GetParent());
        GD.Print("Added bus stop to route. Current path:");
        foreach (var node in LevelState.Routes[^1].PathToTravel)
        {
            GD.Print(node);
        }

        _previewLine = new Line2D();
        _previewLine.AddPoint(GetGlobalMousePosition());
        _currentLevel.AddChild(_previewLine);
        // make states for each of the possible new route states?
        // like "adding first bus stop", "adding subsequent bus stops", "finished route"
    }
}