using Godot;
using System;

public partial class BigBoss : Node2D
{
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");
    [Export] PackedScene laserNode = ResourceLoader.Load<PackedScene>("res://Prefabs/laser_enemy.tscn");

    bool isPositionYTarget = false;
    float positionYTarget = 188;

    bool isLimitHorizontal = false;
    float limitXLeft = 214;
    float limitXRight = 946;

    bool isLimitVertical = false;
    float limitYTop = 130;
    float limitYBottom = 196;

    int maxMoveHorizontal = 3;
    int countMoveHorizontal = 0;
    float rotationLimit = -90;
    bool isRotationLimit = false;
    bool isRotationBigBoss = false;

    Game game;

    Timer timerLaserFire;
    bool isFire = false;
    Sprite2D bigBossBody;

    StaticBody2D laserVerticalBody;
    CollisionShape2D shapeLaserVerticalExternal;
    CollisionShape2D shapeLaserVerticalInternal;

    StaticBody2D laserHorizontalBody;
    CollisionShape2D shapeLaserHorizontalExternal;
    CollisionShape2D shapeLaserHorizontalInterno;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        game = GetParent().GetNode<Game>(".");

        bigBossBody = GetNode<Sprite2D>("BigBossBody");

        isFire = false;

        Callable callableLaser = Callable.From(() => EnableFire());

        timerLaserFire = new Timer();
        timerLaserFire.OneShot = true;
        timerLaserFire.WaitTime = 0.5;
        timerLaserFire.Autostart = true;
        timerLaserFire.Connect("timeout", callableLaser);
        AddChild(timerLaserFire);

        laserVerticalBody = GetNodeOrNull<StaticBody2D>("LaserVerticalBody");
        shapeLaserVerticalExternal = GetNodeOrNull<CollisionShape2D>("LaserVerticalBody/CollisionShape2D");
        shapeLaserVerticalInternal = GetNodeOrNull<CollisionShape2D>("LaserVerticalBody/Area2D/CollisionShape2D");

        laserHorizontalBody = GetNodeOrNull<StaticBody2D>("LaserHorizontalBody");
        shapeLaserHorizontalExternal = GetNodeOrNull<CollisionShape2D>("LaserHorizontalBody/CollisionShape2D");
        shapeLaserHorizontalInterno = GetNodeOrNull<CollisionShape2D>("LaserHorizontalBody/Area2D/CollisionShape2D");

        EnableOrDisableLasers(false);

        game.ShowLifeBarBigBoss();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (isPositionYTarget == false)
        {
            MoveInitialTarget(delta);
        }
        else
        {
            if (countMoveHorizontal == maxMoveHorizontal)
            {
                RotationBigBoss(delta);
                isRotationBigBoss = true;
            }
            else
            {
                isRotationBigBoss = false;
                MoveHorizontal(delta);
            }

            MoveVertical(delta);
        }

        if(isPositionYTarget == true && isRotationBigBoss == false) 
        {
            LaserFire();
            EnableOrDisableLasers(true);
        }
        else
        {
            EnableOrDisableLasers(false);
        }
    }

    private void MoveInitialTarget(double delta)
    {
        Vector2 velocity = new Vector2(0, 50 * (float)delta);
        Position += velocity;
        if (Position.Y >= positionYTarget)
        {
            isPositionYTarget = true;
        }
    }

    private void MoveHorizontal(double delta)
    {
        if (isLimitHorizontal == false)
        {
            Vector2 velocity = new Vector2(50 * (float)delta, 0);
            Position -= velocity;
            if (Position.X <= limitXLeft)
            {
                isLimitHorizontal = true;
                countMoveHorizontal++;
            }
        }
        else
        {
            Vector2 velocity = new Vector2(50 * (float)delta, 0);
            Position += velocity;
            if (Position.X >= limitXRight)
            {
                isLimitHorizontal = false;
                countMoveHorizontal++;
            }
        }
    }

    private void MoveVertical(double delta)
    {
        if (isLimitVertical == false)
        {
            Vector2 velocity = new Vector2(0, 50 * (float)delta);
            Position -= velocity;
            if (Position.Y <= limitYTop)
            {
                isLimitVertical = true;
            }
        }
        else
        {
            Vector2 velocity = new Vector2(0, 50 * (float)delta);
            Position += velocity;
            if (Position.Y >= limitYBottom)
            {
                isLimitVertical = false;
            }
        }
    }

    private void RotationBigBoss(double delta)
    {
        if (isRotationLimit == false)
        {
            RotationDegrees -= 50 * (float)delta;
            if (RotationDegrees <= rotationLimit)
            {
                RotationDegrees = rotationLimit;
                isRotationLimit = true;
                countMoveHorizontal = 0;
            }
        }
        else
        {
            RotationDegrees += 50 * (float)delta;
            if (RotationDegrees >= 0)
            {
                RotationDegrees = 0;
                isRotationLimit = false;
                countMoveHorizontal = 0;
            }
        }
    }

    public void OnNode2DAreaEntered(Node2D area)
    {
        if (area.Name == "LaserBody")
        {
            game.DecrementLifeBigBoss(25);
            if (game.LifeBigBossValueNow <= 0)
            {
                ExplosionBigBoss();
            }
        }
    }

    private void ExplosionBigBoss()
    {
        Node explosionNode = explosion.Instantiate();
        GetParent().AddChild(explosionNode);
        explosionNode.GetNode<Node2D>(explosionNode.GetPath()).Position = new Vector2(Position.X, Position.Y);
        explosionNode.GetNode<Node2D>(explosionNode.GetPath()).Scale = new Vector2(5, 5);
        game.IncrementScore(100000);
        game.BigBossOn = false;
        game.HideLifeBarBigBoss();
        QueueFree();
    }

    private void EnableFire()
    {
        isFire = true;
    }

    private void Fire(int differenceR)
    {
        Node laser = laserNode.Instantiate();
        GetParent().AddChild(laser);
        laser.GetNode<Node2D>(laser.GetPath()).Position = new Vector2(Position.X, Position.Y);
        laser.GetNode<Node2D>(laser.GetPath()).RotationDegrees += RotationDegrees + differenceR;
    }

    private void LaserFire()
    {
        if (isFire == true)
        {
            Fire(30);
            Fire(45);
            Fire(60);
            Fire(120);
            Fire(135);
            Fire(150);
            Fire(210);
            Fire(225);
            Fire(240);
            Fire(300);
            Fire(315);
            Fire(330);
            isFire = false;
            timerLaserFire.Start();
        }
    }

    private void EnableOrDisableLasers(bool isActive)
    {
        laserVerticalBody.Visible = isActive;
        shapeLaserVerticalExternal.SetDeferred("disabled", !isActive);
        shapeLaserHorizontalInterno.SetDeferred("disabled", !isActive);

        laserHorizontalBody.Visible = isActive;
        shapeLaserHorizontalExternal.SetDeferred("disabled", !isActive);
        shapeLaserHorizontalInterno.SetDeferred("disabled",!isActive);
    }
}
