using Godot;
using System;
using System.Reflection.Emit;

public partial class DifficultyIncrease : Node2D
{
    Timer timer;
    bool isInstantiate = false;
    InstantiateEnemies instantiateEnemies;
    InstantiateBoss instantiateBoss;
    
    int level = 1;

    Game game;
    public int Level
    {
        get { return level; }
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        instantiateEnemies = GetParent().GetNode<InstantiateEnemies>("InstantiateEnemies");
        instantiateBoss = GetParent().GetNode<InstantiateBoss>("InstantiateBoss");

        Callable callable = Callable.From(() => IncreaseDifficultyLevel());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 35;
        timer.Autostart = true;
        timer.Connect("timeout", callable);
        AddChild(timer);

        game = GetParent().GetNode<Game>(".");
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{        
        if (isInstantiate == true)
        {
            isInstantiate = false;
            timer.Start();
        }
    }

    private void IncreaseDifficultyLevel()
    {
        level++;
        if(level == 5)
        {
            game.ChangeLevelText("MAX");
            isInstantiate = false;
        }
        else
        {
            game.ChangeLevelText(level.ToString());
            isInstantiate = true;
        }        
        instantiateEnemies.DecrementWaitTimer(level);
        instantiateBoss.DecrementWaitTimer(level);
        
    }
}
