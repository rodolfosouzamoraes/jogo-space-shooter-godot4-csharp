using Godot;
using System;

public partial class Explosion : Node2D
{
    public override void _Ready()
    {
        
    }

    public override void _Process(double delta)
    {
        Vector2 velocity = new Vector2(0, 250 * (float)delta).Rotated(Rotation);
        Position += velocity;
    }

    public void OnAnimationFinished()
    {
        QueueFree();
    }
}
