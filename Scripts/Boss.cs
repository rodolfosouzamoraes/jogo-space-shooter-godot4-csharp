using Godot;
using System;

public partial class Boss : Node2D
{
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");
    [Export] PackedScene laserNode = ResourceLoader.Load<PackedScene>("res://Prefabs/laser_enemy.tscn");

    Timer timer;
    Timer timerFire;
    int enemyLife = 1000;
    bool isFire;
    Sprite2D body;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        body = GetNodeOrNull<Sprite2D>("BossBody");
        ConfigureTimerLife();
        ConfigureTimerFire();
    }

    private void ConfigureTimerLife()
    {
        Callable callable = Callable.From(() => DestroyEnemy());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 20;
        timer.Autostart = true;
        timer.Connect("timeout", callable);
        AddChild(timer);
    }

    private void ConfigureTimerFire()
    {
        Callable callable2 = Callable.From(() => EnableFire());
        isFire = false;
        timerFire = new Timer();
        timerFire.OneShot = true;
        timerFire.WaitTime = 1;
        timerFire.Autostart = true;
        timerFire.Connect("timeout", callable2);
        AddChild(timerFire);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 velocity = new Vector2(0, 50 * (float)delta).Rotated(Rotation);
        Position += velocity;

        if (isFire == true)
        {
            Fire();
            isFire = false;
            timerFire.Start();
        }
    }

    private void DestroyEnemy()
    {
        QueueFree();
    }

    public void OnNode2DAreaEntered(Node2D area)
    {
        if (area.Name == "LaserBody")
        {
            enemyLife -= 25;
            if (enemyLife <= 0)
            {
                ExplosionEnemy();
            }
        }
        else if (area.Name == "PlayerBody" && enemyLife<=25)
        {
            ExplosionEnemy();
        }
    }

    private void ExplosionEnemy()
    {
        Node explosionNode = explosion.Instantiate();
        GetParent().AddChild(explosionNode);
        explosionNode.GetNode<Node2D>(explosionNode.GetPath()).Position = new Vector2(Position.X, Position.Y);
        Game gameNode = GetParent().GetParent().GetNode<Game>(".");
        gameNode.IncrementScore(500);
        DestroyEnemy();
    }

    private void EnableFire()
    {
        isFire = true;
    }

    private void Fire()
    {
        Node laser = laserNode.Instantiate();
        AddChild(laser);
        laser.GetNode<Node2D>(laser.GetPath()).Position = new Vector2(body.Position.X, body.Position.Y+86);
    }


}
