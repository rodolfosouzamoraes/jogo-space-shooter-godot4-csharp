using Godot;
using System;

public partial class InstantiatePowerUps : Node2D
{
    [Export] PackedScene powerUp = ResourceLoader.Load<PackedScene>("res://Prefabs/power_up.tscn");
    [Export] PackedScene powerUpStar = ResourceLoader.Load<PackedScene>("res://Prefabs/power_up_star.tscn");
    [Export] PackedScene powerUpEngine = ResourceLoader.Load<PackedScene>("res://Prefabs/power_up_engine.tscn");
    [Export] PackedScene powerUpShield = ResourceLoader.Load<PackedScene>("res://Prefabs/power_up_shield.tscn");

    Timer timerPowerUp;
    Timer timerPowerUpStar;
    Timer timerPowerUpEngine;
    Timer timerPowerUpShield;
    bool isInstantiatePowerUp = false;
    bool isInstantiatePowerUpStar = false;
    bool isInstantiatePowerUpEngine = false;
    bool isInstantiatePowerUpShield = false;
    public override void _Ready()
    {
        ConfigureTimerPowerUp();
        ConfigureTimerPowerUpStar();
        ConfigureTimerPowerUpEngine();
        ConfigureTimerPowerUpShield();
    }
    private void ConfigureTimerPowerUpShield()
    {
        Callable callable = Callable.From(() => InstantiatePowerUpShield());

        timerPowerUpShield = new Timer();
        timerPowerUpShield.OneShot = true;
        timerPowerUpShield.WaitTime = 90;
        timerPowerUpShield.Autostart = true;
        timerPowerUpShield.Connect("timeout", callable);
        AddChild(timerPowerUpShield);
    }

    private void ConfigureTimerPowerUp()
    {
        Callable callable = Callable.From(() => InstantiatePowerUp());

        timerPowerUp = new Timer();
        timerPowerUp.OneShot = true;
        timerPowerUp.WaitTime = 40;
        timerPowerUp.Autostart = true;
        timerPowerUp.Connect("timeout", callable);
        AddChild(timerPowerUp);
    }

    private void ConfigureTimerPowerUpStar()
    {
        Callable callable = Callable.From(() => InstantiatePowerUpStar());

        timerPowerUpStar = new Timer();
        timerPowerUpStar.OneShot = true;
        timerPowerUpStar.WaitTime = 75;
        timerPowerUpStar.Autostart = true;
        timerPowerUpStar.Connect("timeout", callable);
        AddChild(timerPowerUpStar);
    }

    private void ConfigureTimerPowerUpEngine()
    {
        Callable callable = Callable.From(() => InstantiatePowerUpEngine());

        timerPowerUpEngine = new Timer();
        timerPowerUpEngine.OneShot = true;
        timerPowerUpEngine.WaitTime = 60;
        timerPowerUpEngine.Autostart = true;
        timerPowerUpEngine.Connect("timeout", callable);
        AddChild(timerPowerUpEngine);
    }

    public override void _Process(double delta)
    {
        if (isInstantiatePowerUp == true)
        {
            isInstantiatePowerUp = false;
            timerPowerUp.Start();
        }

        if (isInstantiatePowerUpStar == true)
        {
            isInstantiatePowerUpStar = false;
            timerPowerUpStar.Start();
        }

        if (isInstantiatePowerUpEngine == true)
        {
            isInstantiatePowerUpEngine = false;
            timerPowerUpEngine.Start();
        }

        if (isInstantiatePowerUpShield == true)
        {
            isInstantiatePowerUpShield = false;
            timerPowerUpShield.Start();
        }
    }

    public void InstantiatePowerUp()
    {
        isInstantiatePowerUp = true;
        int positionX = new Random().Next(50, 1101);
        Node powerUpNode = powerUp.Instantiate();
        AddChild(powerUpNode);
        powerUpNode.GetNode<Node2D>(powerUpNode.GetPath()).Position = new Vector2(positionX, -70);
    }

    public void InstantiatePowerUpStar()
    {
        isInstantiatePowerUpStar = true;
        int positionX = new Random().Next(50, 1101);
        Node powerUpStarNode = powerUpStar.Instantiate();
        AddChild(powerUpStarNode);
        powerUpStarNode.GetNode<Node2D>(powerUpStarNode.GetPath()).Position = new Vector2(positionX, -70);
    }

    public void InstantiatePowerUpEngine()
    {
        isInstantiatePowerUpEngine = true;
        int positionX = new Random().Next(50, 1101);
        Node powerUpEngineNode = powerUpEngine.Instantiate();
        AddChild(powerUpEngineNode);
        powerUpEngineNode.GetNode<Node2D>(powerUpEngineNode.GetPath()).Position = new Vector2(positionX, -70);
    }

    public void InstantiatePowerUpShield()
    {
        isInstantiatePowerUpShield = true;
        int positionX = new Random().Next(50, 1101);
        Node powerUpShieldNode = powerUpShield.Instantiate();
        AddChild(powerUpShieldNode);
        powerUpShieldNode.GetNode<Node2D>(powerUpShieldNode.GetPath()).Position = new Vector2(positionX, -70);
    }
}
