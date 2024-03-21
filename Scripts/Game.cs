using Godot;
using System;

public partial class Game : Node
{
	Label score;
	int scoreTotal;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        score = GetNode<Label>("CanvasLayer/Top/Score");
		scoreTotal = 0;
        score.Text = ""+scoreTotal;
	}

	
	public void IncrementScore(int points)
	{
        scoreTotal += points;
        score.Text = "" + scoreTotal;
    }
}
