using Godot;
using System;

public partial class InstantiatePowerUps : Node2D
{
    [Export] PackedScene powerUp = ResourceLoader.Load<PackedScene>("res://Prefabs/power_up.tscn");

    Timer timer;
    bool isInstantiate = false;
    public override void _Ready()
    {
        Callable callable = Callable.From(() => InstantiatePowerUp());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 40;
        timer.Autostart = true;
        timer.Connect("timeout", callable);
        AddChild(timer);
    }

    public override void _Process(double delta)
    {
        if (isInstantiate == true)
        {
            isInstantiate = false;
            timer.Start();
        }
    }

    public void InstantiatePowerUp()
    {
        isInstantiate = true;
        int positionX = new Random().Next(50, 1101);
        Node powerUpNode = powerUp.Instantiate();
        AddChild(powerUpNode);
        powerUpNode.GetNode<Node2D>(powerUpNode.GetPath()).Position = new Vector2(positionX, -70);
    }
}
