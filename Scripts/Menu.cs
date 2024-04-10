using Godot;
using System;

public partial class Menu : Node
{
	Label scoreBest;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scoreBest = GetNode<Label>("CanvasLayer/BestScore");
		scoreBest.Text = PlayerPrefs.GetInt("best_score").ToString();
    }

	public void StartGame()
	{
		GetTree().ChangeSceneToFile("res://Scenes/game.tscn");
	}

	public void ExitGame()
	{
		GetTree().Quit();
	}
}
