using Godot;
using System;

public partial class BigBoss : Node2D
{
	bool isPositionYTarget = false;
	float positionYTarget = 330;

	bool isLimitHorizontal = false;
	float limitXLeft = 214;
	float limitXRight = 946;

    bool isLimitVertical = false;
    float limitYTop = 214;
    float limitYBottom = 348;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(isPositionYTarget == false)
		{
            MoveInitialTarget(delta);
        }
		else
		{
            MoveHorizontal(delta);
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

	private void MoveHorizontal(double delta)
	{
        if (isLimitHorizontal == false)
        {
            Vector2 velocity = new Vector2(50 * (float)delta, 0);
            Position -= velocity;
            if (Position.X <= limitXLeft)
            {
                isLimitHorizontal = true;
            }
        }
        else
        {
            Vector2 velocity = new Vector2(50 * (float)delta, 0);
            Position += velocity;
            if (Position.X >= limitXRight)
            {
                isLimitHorizontal = false;
            }
        }
    }

    private void MoveVertical(double delta)
    {
        if (isLimitVertical == false)
        {
            Vector2 velocity = new Vector2(0,50 * (float)delta);
            Position -= velocity;
            if (Position.Y <= limitYTop)
            {
                isLimitVertical = true;
            }
        }
        else
        {
            Vector2 velocity = new Vector2(0,50 * (float)delta);
            Position += velocity;
            if (Position.Y >= limitYBottom)
            {
                isLimitVertical = false;
            }
        }
    }
}
