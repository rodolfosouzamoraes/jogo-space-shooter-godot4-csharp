using Godot;
using System;
using System.Text;

public partial class Nave : Node2D
{
	[Export] PackedScene laserNode = ResourceLoader.Load<PackedScene>("res://Prefabs/laser.tscn");
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");

    Sprite2D body;
    Timer timer;
    bool isFire;
	int totalPowerUps;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

        body = GetNodeOrNull<Sprite2D>("Body");

		isFire = false;

		Callable callable = Callable.From(() => EnableFire());

		timer = new Timer();
		timer.OneShot = true;
		timer.WaitTime = 0.1;
		timer.Autostart = true;
		timer.Connect("timeout", callable);
		AddChild(timer);

		totalPowerUps = 0;
    }

	private void EnableFire()
	{
		isFire = true;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(isFire == true)
		{
			switch (totalPowerUps)
			{
				case 0:
                    Fire(0, 0, 0);
                    break;
                case 1:
                    Fire(-15, 0, 0);
                    Fire(15, 0, 0);
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
                    Fire(-50, 60, 0);
                    Fire(50, 60, 0);
                    break;
                case 4:
                    Fire(0, 0, 0);
                    Fire(-30, 0, 0);
                    Fire(30, 0, 0);
                    Fire(-50, 60, 0);
                    Fire(50, 60, 0);
                    Fire(-45, 26, -27);
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
                    Fire(-10, 30, -60);
                    Fire(10, 30, 60);
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

	private void Fire(int differenceX, int differenceY, float differenceR)
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
				totalPowerUps++;
				if(totalPowerUps > 5)
				{
					totalPowerUps = 5;
				}
				break;
			case "LaserBody":
            case "PlayerBody":
				return;
			default:
                Game gameNode = GetParent().GetNode<Game>(".");
                gameNode.DecrementLife();
                totalPowerUps = 0;
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

    private void DestroyPlayer()
    {
		QueueFree();
    }
}
