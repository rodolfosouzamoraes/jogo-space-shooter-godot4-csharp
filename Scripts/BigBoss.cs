using Godot;
using System;
using System.Reflection.Emit;

public partial class BigBoss : Node2D
{
    [Export] PackedScene laserNode = ResourceLoader.Load<PackedScene>("res://Prefabs/laser_enemy.tscn");
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");

    bool isPositionYTarget = false;
	float positionYTarget = 188;
	bool isLimitHorizontal = false;
    bool isLimitVertical = false;
	float limitXLeft = 214;
	float limitXRight = 946;
    float limitYTop = 130;
    float limitYBottom = 196;
    int maxMoveHorizontal = 3; //5
    int countMoveHorizontal = 0;
    float rotationLimit = -90;
    bool isRotationLimit = false;

    Game game;

    StaticBody2D laserVerticalBody;
    CollisionShape2D shapeLaserVerticalExternal;
    CollisionShape2D shapeLaserVerticalInternal;

    StaticBody2D laserHorizontalBody;
    CollisionShape2D shapeLaserHorizontalExternal;
    CollisionShape2D shapeLaserHorizontalInternal;

    bool isRotationBigBoss = false;

    Timer timerLaserFire;
    bool isFire;
    Sprite2D bigBossBody;

    AudioStreamPlayer2D audioLaser;
    AudioStreamPlayer2D audioHit;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        game = GetParent().GetNode<Game>(".");
        game.ShowLifeBarBigBoss();

        laserVerticalBody = GetNodeOrNull<StaticBody2D>("LaserVerticalBody");
        shapeLaserVerticalExternal = GetNodeOrNull<CollisionShape2D>("LaserVerticalBody/CollisionShape2D");
        shapeLaserVerticalInternal = GetNodeOrNull<CollisionShape2D>("LaserVerticalBody/Area2D/CollisionShape2D");

        laserHorizontalBody = GetNodeOrNull<StaticBody2D>("LaserHorizontalBody");
        shapeLaserHorizontalExternal = GetNodeOrNull<CollisionShape2D>("LaserHorizontalBody/CollisionShape2D");
        shapeLaserHorizontalInternal = GetNodeOrNull<CollisionShape2D>("LaserHorizontalBody/Area2D/CollisionShape2D");

        EnableOrDisableLasers(false);

        bigBossBody = GetNodeOrNull<Sprite2D>("BigBossBody");

        Callable callableLaser = Callable.From(() => EnableFire());

        timerLaserFire = new Timer();
        timerLaserFire.OneShot = true;
        timerLaserFire.WaitTime = 0.5;
        timerLaserFire.Autostart = true;
        timerLaserFire.Connect("timeout", callableLaser);
        AddChild(timerLaserFire);

        audioLaser = GetNodeOrNull<AudioStreamPlayer2D>("AudioLaser");
        audioHit = GetNodeOrNull<AudioStreamPlayer2D>("AudioHit");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        MoveAI(delta);
        LaserPower();
    }

    private void MoveAI(double delta)
    {
        if (isPositionYTarget == false)
        {
            MoveInitialTarget(delta);
        }
        else
        {
            if(countMoveHorizontal == maxMoveHorizontal)
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
            Vector2 velocity = new Vector2(0, 75 * (float)delta);
            Position += velocity;
            if (Position.Y >= limitYBottom)
            {
                isLimitVertical = false;
            }
        }
    }

    public void MoveHorizontal(double delta)
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

    private void RotationBigBoss(double delta)
    {
        if (isRotationLimit == false)
        {
            RotationDegrees -= 50 * (float)delta;
            if (RotationDegrees <= -90)
            {
                isRotationLimit = true;
                countMoveHorizontal = 0;
            }
        }
        else
        {
            RotationDegrees += 50 * (float)delta;
            if (RotationDegrees >= 0)
            {
                isRotationLimit = false;
                countMoveHorizontal = 0;
            }
        }
    }

    public void OnNode2DAreaEntered(Node2D area)
	{
		
		if(area.Name == "LaserBody")
		{
            audioHit.Play();
            game.DecrementLifeBigBoss(25);
            if (game.LifeBigBossValueNow <= 0)
            {
                ExplosionEnemy();
            }
        }
	}

    private void EnableOrDisableLasers(bool isActive)
    {
        laserVerticalBody.Visible = isActive;
        shapeLaserVerticalExternal.SetDeferred("disabled", !isActive);
        shapeLaserVerticalInternal.SetDeferred("disabled", !isActive);

        laserHorizontalBody.Visible = isActive;
        shapeLaserHorizontalExternal.SetDeferred("disabled", !isActive);
        shapeLaserHorizontalInternal.SetDeferred("disabled", !isActive);
    }

    private void LaserPower()
    {
        if (isPositionYTarget == true && isRotationBigBoss == false)
        {
            EnableOrDisableLasers(true);
            LaserFire();
        }
        else
        {
            EnableOrDisableLasers(false);
        }
    }

    private void LaserFire()
    {
        if (isFire == true)
        {
            audioLaser.Play();
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

    public void ExplosionEnemy()
    {
        Node explosionNode = explosion.Instantiate();
        GetParent().AddChild(explosionNode);
        explosionNode.GetNode<Node2D>(explosionNode.GetPath()).Position = new Vector2(Position.X, Position.Y);
        explosionNode.GetNode<Node2D>(explosionNode.GetPath()).Scale = new Vector2(5, 5);
        Game game = GetParent().GetNode<Game>(".");
        game.IncrementScore(100000);
        game.BigBossOn = false;
        game.DisableLifeBarBigBoss();
        DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        QueueFree();
    }
}
