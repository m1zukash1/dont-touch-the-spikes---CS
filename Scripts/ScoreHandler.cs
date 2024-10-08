using Godot;
using System;

public partial class ScoreHandler : Node2D
{
	[ExportGroup("Node References")]
	[Export]
	private Label ScoreLabel;
	[Export]
	private Bird Bird;
	[Export]
	private MenuController MenuController;
	public int Score = 0;
	public override void _Ready()
	{
		Bird.HitWall += OnBirdHitWall;
		MenuController.GameStarted += OnGameStarted;
	}

    private void OnGameStarted()
    {
        ScoreLabel.SetText("00");
    }

    public void OnBirdHitWall()
	{
		Score++;
		ScoreLabel.SetText(Score.ToString().PadZeros(2));
	}
	public override void _Process(double delta)
	{
	}
}
