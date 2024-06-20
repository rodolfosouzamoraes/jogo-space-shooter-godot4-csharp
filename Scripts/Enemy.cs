using Godot;
using System;

public partial class Enemy : Node2D
{
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");

    Timer timer;
    float enemyLife = 100;

    Sprite2D body;
    CompressedTexture2D textureNv3 = ResourceLoader.Load<CompressedTexture2D>("res://Sprites/SkinEnemies/enemyGreen1.png");
    CompressedTexture2D textureNv4 = ResourceLoader.Load<CompressedTexture2D>("res://Sprites/SkinEnemies/enemyRed1.png");
    CompressedTexture2D textureNv5 = ResourceLoader.Load<CompressedTexture2D>("res://Sprites/SkinEnemies/enemyBlack1.png");

    AudioStreamPlayer2D audioHit;

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

        body = GetNode<Sprite2D>("Body");

        audioHit = GetNodeOrNull<AudioStreamPlayer2D>("AudioHit");
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
        //GD.Print($"Colidiu: {area.Name}");
        if(area.Name == "LaserBody")
        {
            audioHit.Play();
            enemyLife -= 25;
            if(enemyLife <= 0)
            {
                ExplosionEnemy();
            }
        }
        else if (area.Name == "PlayerBody" || area.Name == "PlayerBodyShield")
        {
            Nave nave = GetNode<Nave>(area.GetParent().GetParent().GetPath());
            nave.Damage();
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

    public void SetSkinEnemy(int level)
    {
        switch(level)
        {
            case 3:
                body.Texture = textureNv3;
                break;
            case 4:
                body.Texture = textureNv4;
                break;
            case 5:
                body.Texture = textureNv5;
                break;
        }
    }

    public void SetLifeEnemy(float life)
    {
        enemyLife = life;
    }
}
