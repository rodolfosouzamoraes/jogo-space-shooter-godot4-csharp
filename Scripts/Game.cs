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

    Node2D gameOverNode;
    Label scoreNow;
    Label scoreBest;
    int scoreBestTotal;

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

        scoreNow = GetNode<Label>("CanvasLayer/GameOver/ScoreFinal");
        scoreBest = GetNode<Label>("CanvasLayer/GameOver/BestScore");
        gameOverNode = GetNode<Node2D>("CanvasLayer/GameOver");

        scoreBestTotal = PlayerPrefs.GetInt("best_score");
        if (scoreBestTotal == 0)
        {
            PlayerPrefs.SetInt("best_score", 0);
        }
        scoreBest.Text = scoreBestTotal.ToString();
        gameOverNode.Hide();
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
                ShowGameOver();
                break;
        }
    }

    private void ShowGameOver()
    {
        scoreNow.Text = "" + scoreTotal;
        
        if(scoreTotal > scoreBestTotal)
        {
            scoreBestTotal = scoreTotal;
            PlayerPrefs.SetInt("best_score", scoreBestTotal);
            scoreBest.Text = scoreBestTotal.ToString();
        }

        gameOverNode.Show();
    }

    public void TryAgain()
    {
        GetTree().ReloadCurrentScene();
    }

    public void GotoMenu()
    {

    }
}
