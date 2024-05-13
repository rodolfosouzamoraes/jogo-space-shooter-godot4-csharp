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
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        game = GetParent().GetNode<Game>(".");
        game.BigBossOn = true;

        laserVerticalBody = GetNodeOrNull<StaticBody2D>("LaserVerticalBody");
        shapeLaserVerticalExternal = GetNodeOrNull<CollisionShape2D>("LaserVerticalBody/CollisionShape2D");
        shapeLaserVerticalInternal = GetNodeOrNull<CollisionShape2D>("LaserVerticalBody/Area2D/CollisionShape2D");

        laserHorizontalBody = GetNodeOrNull<StaticBody2D>("LaserHorizontalBody");
        shapeLaserHorizontalExternal = GetNodeOrNull<CollisionShape2D>("LaserHorizontalBody/CollisionShape2D");
        shapeLaserHorizontalInternal = GetNodeOrNull<CollisionShape2D>("LaserHorizontalBody/Area2D/CollisionShape2D");

        EnableOrDisableLasers(false);
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
		
		if(area.Name == "PlayerBody" ||  area.Name == "LaserBody")
		{
            GD.Print($"BigBoss Colidiu: {area.Name}");
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
        }
        else
        {
            EnableOrDisableLasers(false);
        }
    }
}
