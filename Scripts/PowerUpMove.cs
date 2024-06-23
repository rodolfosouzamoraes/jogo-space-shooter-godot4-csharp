using Godot;
using System;

public partial class PowerUpMove : Node2D
{
    [Export] int idPowerUp;
    Timer timer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Callable callable = Callable.From(() => DestroyPowerUp());

        timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 5;
        timer.Autostart = true;
        timer.Connect("timeout", callable);
        AddChild(timer);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 velocity = new Vector2(0, 250 * (float)delta).Rotated(Rotation);
        Position += velocity;
    }

    private void DestroyPowerUp()
    {
        QueueFree();
    }

    public void OnNode2DAreaEntered(Node2D area)
    {
        if (area.Name == "PlayerBody")
        {
            Game.Instance.PlayAudioPowerUp();
            Nave nave = GetNode<Nave>(area.GetParent().GetParent().GetPath());
            switch (idPowerUp)
            {
                case 1:
                    nave.IncrementPowerUpLaser();
                    break;
                case 2:
                    nave.IncrementPowerUpScore();
                    break;
                case 3:
                    nave.IncrementEngine();
                    break;
                case 4:
                    nave.EnableShield();
                    break;
            }
            DestroyPowerUp();
        }
    }
}
