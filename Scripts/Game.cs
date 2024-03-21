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
		score.Text = scoreTotal.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void IncrementScore(int points)
	{
		scoreTotal += points;
		score.Text = scoreTotal.ToString();
	}
}
