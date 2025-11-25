using Godot;
using System;

public partial class PreviewBusStop : Node2D
{
    public Area2D IntersectionDetector;

    public override void _Ready()
    {
        IntersectionDetector = GetNode<Area2D>("IntersectionDetector");
    }

    /// <summary>
    /// Checks if the preview bus stop is overlapping with any IntersectionNodes.
    /// For the purpose of preventing bus stops from being placed on intersections.
    /// </summary>
    /// <returns></returns>
    public bool IsOverlappingIntersection()
    {
        return IntersectionDetector.HasOverlappingAreas();
    }
}
