﻿using System;
using Godot;


public partial class Laser : Node2D
{
    Timer timer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Callable callable = Callable.From(() => DestroyLaser());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 1;
        timer.Autostart = true;
        timer.Connect("timeout", callable);
        AddChild(timer);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 velocity = new Vector2(0, -1 * 500 * (float)delta).Rotated(Rotation);
        Position += velocity;
    }

    private void DestroyLaser()
    {
        QueueFree();
    }

    public void OnNode2DAreaEntered(Node2D area)
    {
        if(area.Name != "LaserBody")
        {
            DestroyLaser();
        }        
    }
}
