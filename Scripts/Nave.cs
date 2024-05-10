using Godot;
using System;

public partial class Nave : Node2D
{
	[Export] PackedScene laserNode = ResourceLoader.Load<PackedScene>("res://Prefabs/laser.tscn");
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");


    Sprite2D body;
	CharacterBody2D shieldBody;
	CollisionShape2D collisionShapeExternal;
	CollisionShape2D collisionShapeInternal;
    Timer timer;
	Timer timerShield;
    bool isFire;
	int totalPowerUp;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        body = GetNodeOrNull<Sprite2D>("Body");
		shieldBody = GetNodeOrNull<CharacterBody2D>("Body/PlayerBodyShield");
		collisionShapeExternal = GetNodeOrNull<CollisionShape2D>("Body/PlayerBodyShield/CollisionShape2D");
		collisionShapeInternal = GetNodeOrNull<CollisionShape2D>("Body/PlayerBodyShield/Area2D/CollisionShape2D");
        EnableOrDisableShield(false);

        collisionShapeExternal = GetNodeOrNull<CollisionShape2D>("Body/PlayerBodyShield/CollisionShape2D");
		collisionShapeInternal = GetNodeOrNull<CollisionShape2D>("Body/PlayerBodyShield/Area2D/CollisionShape2D2");
		collisionShapeInternal.SetDeferred("disabled", true);
        collisionShapeExternal.SetDeferred("disabled", true);

		ConfigureTimerFire();
		ConfigureTimerShield();

        totalPowerUp = 0;
    }

	private void ConfigureTimerShield()
	{
        Callable callable = Callable.From(() => EnableOrDisableShield(false));

        timerShield = new Timer();
        timerShield.OneShot = true;
        timerShield.WaitTime = 10;
        timerShield.Autostart = true;
        timerShield.Connect("timeout", callable);
        AddChild(timerShield);
    }

	private void ConfigureTimerFire()
	{
        Callable callable = Callable.From(() => EnableFire());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 0.1;
        timer.Autostart = true;
        timer.Connect("timeout", callable);
        AddChild(timer);
    }

	private void EnableFire()
	{
		isFire = true;
    }

	private void EnableOrDisableShield(bool isActive)
	{
		shieldBody.Visible = isActive;
		collisionShapeInternal.SetDeferred("disabled", !isActive);
		collisionShapeExternal.SetDeferred("disabled", !isActive);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(isFire == true)
		{
			switch (totalPowerUp)
			{
				case 0:
					Fire(0, 0, 0);
					break;
				case 1:
					Fire(-15,0,0);
					Fire(15,0,0);
					break;
				case 2:
					Fire(0,0,0);
					Fire(-30,0,0);
					Fire(30,0,0);
					break;
				case 3:
                    Fire(0, 0, 0);
                    Fire(-30, 0, 0);
                    Fire(30, 0, 0);
					Fire(-50,60,0);
					Fire(50,60,0);
                    break;
				case 4:
                    Fire(0, 0, 0);
                    Fire(-30, 0, 0);
                    Fire(30, 0, 0);
                    Fire(-50, 60, 0);
                    Fire(50, 60, 0);
					Fire(-45,26,-27);
					Fire(45, 26, 27);
                    break;
				case 5:
                    Fire(0, 0, 0);
                    Fire(-30, 0, 0);
                    Fire(30, 0, 0);
                    Fire(-50, 60, 0);
                    Fire(50, 60, 0);
                    Fire(-45, 26, -27);
                    Fire(45, 26, 27);
					Fire(-10,30,-60);
					Fire(10,30,60);
                    break;
			}

			isFire = false;
			timer.Start();
        }
		
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion motionEvent)
		{
            body.Position = motionEvent.Position;
        }
    }

	private void Fire(int differenceX, int differenceY, int differenceR)
	{
		Node laser = laserNode.Instantiate();
		AddChild(laser);
		laser.GetNode<Node2D>(laser.GetPath()).Position = new Vector2(body.Position.X + differenceX, body.Position.Y + differenceY);
		laser.GetNode<Node2D>(laser.GetPath()).RotationDegrees += differenceR;
    }

	public void OnNode2DAreaEntered(Node2D area)
	{
		switch (area.Name)
		{
			case "PowerUpBody":
				totalPowerUp++;
				if(totalPowerUp > 5)
				{
					totalPowerUp = 5;
				}
				break;
			case "LaserBody":
			case "PlayerBody":
			case "PlayerBodyShield":
				break;
			case "PowerUpShieldBody":
				EnableOrDisableShield(true);
				timerShield.Start();
                break;
			case "PowerUpEngineBody":
                Game gameNode3 = GetParent().GetNode<Game>(".");
				gameNode3.IncrementLife();
                break;
			case "PowerUpStarBody":
                Game gameNode2 = GetParent().GetNode<Game>(".");
				gameNode2.IncrementScore(5000);
                break;
			default:
				if (shieldBody.Visible == true) return;
                Game gameNode = GetParent().GetNode<Game>(".");
                gameNode.DecrementLife();
				totalPowerUp = 0;
                if (gameNode.LifePlayer < 0)
                {
                    Node explosionNode = explosion.Instantiate();
                    GetParent().AddChild(explosionNode);
                    explosionNode.GetNode<Node2D>(explosionNode.GetPath()).Position = new Vector2(body.Position.X, body.Position.Y);
                    DestroyPlayer();
                }
				break;
		}
	}

	public void DestroyPlayer()
	{
		QueueFree();
	}
}
