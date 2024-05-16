using Godot;
using System;

public partial class BigBoss : Node2D
{
    [Export] PackedScene explosion = ResourceLoader.Load<PackedScene>("res://Prefabs/explosion.tscn");

    bool isPositionYTarget = false;
	float positionYTarget = 330;

	bool isLimitHorizontal = false;
	float limitXLeft = 214;
	float limitXRight = 946;

    bool isLimitVertical = false;
    float limitYTop = 214;
    float limitYBottom = 348;

    int maxMoveHorizontal = 3;
    int countMoveHorizontal = 0;
    float rotationLimit = -90;
    bool isRotationLimit = false;
    bool isRotationBigBoss = false;

    Game game;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        game = GetParent().GetNode<Game>(".");
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

    private void RotationBigBoss(double delta)
    {
        if(isRotationLimit == false)
        {
            RotationDegrees -= 50 * (float)delta;
            if(RotationDegrees <= rotationLimit)
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
        if(area.Name == "LaserBody")
        {
            game.DecrementLifeBigBoss(25);
            if(game.LifeBigBossValueNow <= 0)
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
        explosionNode.GetNode<Node2D>(explosionNode.GetPath()).Scale = new Vector2(5,5);
        game.IncrementScore(100000);
        game.BigBossOn = false;
        game.HideLifeBarBigBoss();
        QueueFree();
    }
}
