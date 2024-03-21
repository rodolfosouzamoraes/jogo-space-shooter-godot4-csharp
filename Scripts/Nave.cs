using Godot;
using System;

public partial class Nave : Node2D
{
	[Export] PackedScene laserNode = ResourceLoader.Load<PackedScene>("res://Prefabs/laser.tscn");
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");

    Sprite2D body;
    Timer timer;
    bool isFire;

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
            Fire();
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

	private void Fire()
	{
		Node laser = laserNode.Instantiate();
		AddChild(laser);
		laser.GetNode<Node2D>(laser.GetPath()).Position = new Vector2(body.Position.X, body.Position.Y);
	}

	public void OnNode2DAreaEntered(Node2D area)
	{
		if(area.Name!= "LaserBody" && area.Name!= "PlayerBody")
		{
            Game gameNode = GetParent().GetNode<Game>(".");
            gameNode.DecrementLife();
			if(gameNode.LifePlayer < 0)
			{
                Node explosionNode = explosion.Instantiate();
                GetParent().AddChild(explosionNode);
                explosionNode.GetNode<Node2D>(explosionNode.GetPath()).Position = new Vector2(body.Position.X, body.Position.Y);
				DestroyPlayer();
            }
        }
	}

    private void DestroyPlayer()
    {
		QueueFree();
    }
}
