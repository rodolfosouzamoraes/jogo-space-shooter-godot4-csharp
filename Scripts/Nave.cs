using Godot;
using System;

public partial class Nave : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion motionEvent)
		{
            Sprite2D body = GetNodeOrNull<Sprite2D>("Body");
            body.Position = motionEvent.Position;
        }
    }
}
