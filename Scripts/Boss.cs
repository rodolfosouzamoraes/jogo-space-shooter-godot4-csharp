using Godot;
using System;

public partial class Boss : Node2D
{
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");
    [Export] PackedScene laserNode = ResourceLoader.Load<PackedScene>("res://Prefabs/laser_enemy.tscn");

    Timer timer;
    int enemyLife = 1000;

    Timer timerLaser;
    bool isFire;

    Sprite2D body;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Callable callable = Callable.From(() => DestroyEnemy());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 20;
        timer.Autostart = true;
        timer.Connect("timeout", callable);
        AddChild(timer);

        isFire = false;

        Callable callableLaser = Callable.From(() => EnableFire());

        timerLaser = new Timer();
        timerLaser.OneShot = true;
        timerLaser.WaitTime = 1;
        timerLaser.Autostart = true;
        timerLaser.Connect("timeout", callableLaser);
        AddChild(timerLaser);

        body = GetNodeOrNull<Sprite2D>("BossBody");
    }

    private void EnableFire()
    {
        isFire = true;
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
            timerLaser.Start();
        }
    }

    private void Fire()
    {
        Node laser = laserNode.Instantiate();
        AddChild(laser);
        laser.GetNode<Node2D>(laser.GetPath()).Position = new Vector2(body.Position.X, body.Position.Y+86);
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
        else if (area.Name == "PlayerBody")
        {
            ExplosionEnemy();
        }
    }

    public void ExplosionEnemy()
    {
        Node explosionNode = explosion.Instantiate();
        GetParent().AddChild(explosionNode);
        explosionNode.GetNode<Node2D>(explosionNode.GetPath()).Position = new Vector2(Position.X, Position.Y);
        Game game = GetParent().GetParent().GetNode<Game>(".");
        game.IncrementScore(500);
        DestroyEnemy();
    }
}