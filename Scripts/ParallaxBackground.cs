using System;
using Godot;

public partial class ParallaxBackground : Godot.ParallaxBackground
{
    double offsetloc = 0;
    public override void _Ready()
    {
        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        offsetloc += 150 * delta;
        Vector2 motionMirroring = new Vector2(0, (float)offsetloc);
        ScrollOffset = motionMirroring;
    }
}
