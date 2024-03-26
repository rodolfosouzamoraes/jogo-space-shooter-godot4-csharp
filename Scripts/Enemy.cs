using Godot;
using System;

public partial class Enemy : Node2D
{
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");

    Timer timer;
    int enemyLife = 100;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Callable callable = Callable.From(() => DestroyEnemy());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 5;
        timer.Autostart = true;
        timer.Connect("timeout", callable);
        AddChild(timer);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 velocity = new Vector2(0, 250 * (float)delta).Rotated(Rotation);
        Position += velocity;
    }

    private void DestroyEnemy()
    {
        QueueFree();
    }

    public void OnNode2DAreaEntered(Node2D area)
    {
        if(area.Name == "LaserBody")
        {
            enemyLife -= 25;
            if(enemyLife <= 0)
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
        game.IncrementScore(125);
        DestroyEnemy();
    }
}
