using Godot;
using System;

public partial class InstantiateBoss : Node2D
{
    [Export] PackedScene enemy = ResourceLoader.Load<PackedScene>("res://Prefabs/boss.tscn");

    Timer timer;
    bool isInstantiate = false;
    public override void _Ready()
    {
        Callable callable = Callable.From(() => InstantiateEnemy());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 30;
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

    public void InstantiateEnemy()
    {
        isInstantiate = true;
        int positionX = new Random().Next(50, 1101);
        Node enemyNode = enemy.Instantiate();
        AddChild(enemyNode);
        enemyNode.GetNode<Node2D>(enemyNode.GetPath()).Position = new Vector2(positionX, -70);
    }
}
