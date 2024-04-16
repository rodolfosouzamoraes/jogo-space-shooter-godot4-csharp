using Godot;
using System;

public partial class DifficultyIncrease : Node2D
{
    Timer timer;
    bool isInstantiate = false;
    InstantiateEnemies instantiateEnemies;
    InstantiateBoss instantiateBoss;
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
        isInstantiate = true;
        instantiateEnemies.DecrementWaitTimer();
        instantiateBoss.DecrementWaitTimer();
    }
}
