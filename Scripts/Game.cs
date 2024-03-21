using Godot;
using System;

public partial class Game : Node
{
	Label score;
	int scoreTotal;
	Sprite2D lifeOn1;
	Sprite2D lifeOn2;
	Sprite2D lifeOn3;
    int lifePlayer;

    public int LifePlayer
    {
        get { return lifePlayer; }
        set { lifePlayer = value; }
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        score = GetNode<Label>("CanvasLayer/Top/Score");
		scoreTotal = 0;
        score.Text = scoreTotal.ToString();

		lifeOn1 = GetNode<Sprite2D>("CanvasLayer/Top/LifeOn1");
        lifeOn2 = GetNode<Sprite2D>("CanvasLayer/Top/LifeOn2");
        lifeOn3 = GetNode<Sprite2D>("CanvasLayer/Top/LifeOn3");
        lifePlayer = 3;
    }


    public void IncrementScore(int points)
	{
        scoreTotal += points;
        score.Text = "" + scoreTotal;
    }

    public void DecrementLife()
    {
        lifePlayer--;
        switch (lifePlayer)
        {
            case 2:
                lifeOn3.Hide();
                break;
            case 1:
                lifeOn2.Hide();
                break;
            case 0:
                lifeOn1.Hide();
                break;
            default: GD.Print("GameOver!");

                break;
        }
    }
}
