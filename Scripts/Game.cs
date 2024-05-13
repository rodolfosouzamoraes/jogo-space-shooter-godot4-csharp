using Godot;
using System;

public partial class Game : Node
{
    [Export] PackedScene bigBoss = ResourceLoader.Load<PackedScene>("res://Prefabs/big_boss.tscn");

    Label score;
	int scoreTotal;
	Sprite2D lifeOn1;
	Sprite2D lifeOn2;
	Sprite2D lifeOn3;
	int lifePlayer;

	Node2D gameOverNode;
	Label scoreFinal;
	Label scoreBest;
	int scoreBestTotal;

	Node2D pauseNode;

	Label levelNow;

	bool bigBossOn = false;

	Node2D lifeBigBoss;
	ProgressBar lifeBigBossBar;

	int lifeBigBossValueMax = 50000;
	int lifeBigBossValueNow;
    Timer timerBigBoss;
    bool isInstantiateBigBoss = false;
    double waitTimerInstantiate = 180;


    public int LifePlayer
	{
		get { return lifePlayer; }
	}

	public bool BigBossOn
	{
		get { return bigBossOn; }
		set { bigBossOn = value; }
	}

	public int LifeBigBossValueNow
	{
		get { return lifeBigBossValueNow; }
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

		scoreFinal = GetNode<Label>("CanvasLayer/GameOver/ScoreFinal");
		scoreBest = GetNode<Label>("CanvasLayer/GameOver/BestScore");
		gameOverNode = GetNode<Node2D>("CanvasLayer/GameOver");

		scoreBestTotal = PlayerPrefs.GetInt("best_score");
		if(scoreBestTotal == 0)
		{
			PlayerPrefs.SetInt("best_score", 0);
		}
		scoreBest.Text = scoreBestTotal.ToString();
		gameOverNode.Hide();

		pauseNode = GetNode<Node2D>("CanvasLayer/PauseGame");
		pauseNode.Hide();

		levelNow = GetNode<Label>("CanvasLayer/Top/Label");

		ConfigureBigBoss();
    }

	private void ConfigureBigBoss()
	{
        lifeBigBoss = GetNode<Node2D>("CanvasLayer/Top/LifeBigBoss");
        lifeBigBossBar = GetNode<ProgressBar>("CanvasLayer/Top/LifeBigBoss/lifeBigBosBar");
        lifeBigBossBar.MaxValue = lifeBigBossValueMax;
        lifeBigBossValueNow = lifeBigBossValueMax;
        lifeBigBossBar.Value = lifeBigBossValueNow;
        DisableLifeBarBigBoss();

        Callable callable = Callable.From(() => InstantiateBigBoss());

        timerBigBoss = new Timer();
        timerBigBoss.OneShot = true;
        timerBigBoss.WaitTime = waitTimerInstantiate;
        timerBigBoss.Autostart = true;
        timerBigBoss.Connect("timeout", callable);
        AddChild(timerBigBoss);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (isInstantiateBigBoss == true)
        {
            isInstantiateBigBoss = false;
            timerBigBoss.Start();
        }
    }

	public void IncrementScore(int points)
	{
		scoreTotal += points;
		score.Text = scoreTotal.ToString();
	}

	public void DecrementLife()
	{
		lifePlayer--;
		switch (lifePlayer)
        {
			case 2: lifeOn3.Hide();
				break;
			case 1: lifeOn2.Hide();
				break;
			case 0: lifeOn1.Hide();
				break;
			default:
				ShowGameOver();
				break;
		}
	}

    public void IncrementLife()
    {
        lifePlayer++;
		if(lifePlayer > 3)
		{
			lifePlayer = 3;
		}
        switch (lifePlayer)
        {
            case 3:
                lifeOn3.Show();
                break;
            case 2:
                lifeOn2.Show();
                break;
            case 1:
                lifeOn1.Show();
                break;
        }
    }

    private void ShowGameOver()
	{
		scoreFinal.Text = scoreTotal.ToString();

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
		GetTree().ChangeSceneToFile("res://Scenes/menu.tscn");
	}

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
			{
				PausedGame();
			}
		}
    }

	public void PausedGame()
	{
		pauseNode.Show();
		GetTree().Paused = true;
	}

	public void ContinueGame()
	{
		pauseNode.Hide();
		GetTree().Paused = false;
	}

	public void ChangeLevelText(string text)
	{
		levelNow.Text = text;
	}

	public void DecrementLifeBigBoss(int value)
	{
		lifeBigBossValueNow -= value;
        lifeBigBossBar.Value = lifeBigBossValueNow;
    }

	public void ShowLifeBarBigBoss()
	{
        lifeBigBoss.Show();
        lifeBigBossValueNow = lifeBigBossValueMax;
        lifeBigBossBar.Value = lifeBigBossValueNow;
    }

	public void DisableLifeBarBigBoss()
	{
		lifeBigBoss.Hide();
	}

    private void InstantiateBigBoss()
    {
		if(bigBossOn == false)
		{
			isInstantiateBigBoss = true;
            Node bossNode = bigBoss.Instantiate();
            AddChild(bossNode);
            bossNode.GetNode<Node2D>(bossNode.GetPath()).Position = new Vector2(558, -452);
            bigBossOn = true;
        }        
    }
}
