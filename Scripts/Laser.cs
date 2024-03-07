using System;
using Godot;


public partial class Laser : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 velocity = new Vector2(0, -1 * 500 * (float)delta).Rotated(Rotation);
        Position += velocity;
    }

}
