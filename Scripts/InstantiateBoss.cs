using Godot;
using System;

public partial class InstantiateBoss : Node2D
{
    [Export] PackedScene boss = ResourceLoader.Load<PackedScene>("res://Prefabs/boss.tscn");

    Timer timer;
    bool isInstantiate = false;
    double waitTimerInstantiate = 30;

    int levelNow = 1;
    float lifeBoss = 1000;
    public override void _Ready()
    {
        Callable callable = Callable.From(() => InstantiateEnemy());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = waitTimerInstantiate;
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
        int positionX = new Random().Next(86, 1080);
        Node enemyNode = boss.Instantiate();
        AddChild(enemyNode);
        enemyNode.GetNode<Node2D>(enemyNode.GetPath()).Position = new Vector2(positionX, -102);
        Boss bossInstance = enemyNode.GetNode<Boss>(enemyNode.GetPath());
        bossInstance.SetSkinBoss(levelNow);
        bossInstance.SetLifeBoss(lifeBoss);
    }

    public void DecrementWaitTimer(int level)
    {
        waitTimerInstantiate -= 2;
        if (waitTimerInstantiate < 15)
        {
            waitTimerInstantiate = 15;
        }
        timer.WaitTime = waitTimerInstantiate;

        levelNow = level;
        lifeBoss = lifeBoss * 1.5f;
        GD.Print($"Life Boss Now: {lifeBoss}");
    }
}
