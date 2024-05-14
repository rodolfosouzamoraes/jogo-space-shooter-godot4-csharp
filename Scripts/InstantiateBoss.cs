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

    Game game;
    public override void _Ready()
    {
        Callable callable = Callable.From(() => InstantiateNewBoss());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = waitTimerInstantiate;
        timer.Autostart = true;
        timer.Connect("timeout", callable);
        AddChild(timer);

        game = GetParent().GetNode<Game>(".");
    }

    public override void _Process(double delta)
    {
        if (isInstantiate == true)
        {
            isInstantiate = false;
            timer.Start();
        }
    }

    public void InstantiateNewBoss()
    {
        isInstantiate = true;
        if (game.BigBossOn == true) { return; }
        int positionX = new Random().Next(50, 1101);
        Node bossNode = boss.Instantiate();
        AddChild(bossNode);
        bossNode.GetNode<Node2D>(bossNode.GetPath()).Position = new Vector2(positionX, -70);
        Boss bossInstance = bossNode.GetNode<Boss>(bossNode.GetPath());
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
    }
}
