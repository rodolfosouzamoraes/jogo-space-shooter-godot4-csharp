using Godot;
using System;
using System.Reflection.Emit;

public partial class BigBoss : Node2D
{
	bool isPositionYTarget = false;
	float positionYTarget = 330;
	bool isLimitHorizontal = false;
    bool isLimitVertical = false;
	float limitXLeft = 214;
	float limitXRight = 946;
    float limitYTop = 214;
    float limitYBottom = 348;
    int maxMoveHorizontal = 3; //5
    int countMoveHotizontal = 0;
    float rotationLimit = -90;
    bool isRotationLimit = false;

    Game game;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        game = GetParent().GetNode<Game>(".");
        game.BigBossOn = true;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        MoveAI(delta);
    }

    private void MoveAI(double delta)
    {
        if (isPositionYTarget == false)
        {
            MoveInitialTarget(delta);
        }
        else
        {
            if(countMoveHotizontal == maxMoveHorizontal)
            {
                RotationBigBoss(delta);                
            }
            else
            {
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
                countMoveHotizontal++;
            }
        }
        else
        {
            Vector2 velocity = new Vector2(50 * (float)delta, 0);
            Position += velocity;
            if (Position.X >= limitXRight)
            {
                isLimitHorizontal = false;
                countMoveHotizontal++;
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
                countMoveHotizontal = 0;
            }
        }
        else
        {
            RotationDegrees += 50 * (float)delta;
            if (RotationDegrees >= 0)
            {
                isRotationLimit = false;
                countMoveHotizontal = 0;
            }
        }
    }

    public void OnNode2DAreaEntered(Node2D area)
	{
		
		if(area.Name == "PlayerBody" ||  area.Name == "LaserBody")
		{
            GD.Print($"BigBoss Colidiu: {area.Name}");
        }
	}
}
