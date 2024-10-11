using Godot;
using System;

public partial class DeathScreenController : Control
{
	[ExportGroup("Node References")]
	[Export]
	ScoreManager ScoreManager;
	[Export]
	CandyManager CandyManager;
	[Export]
	Bird Bird;
	[Export]
	Button ReplayButton;
	[Export]
	Label ScoreLabel;
	[Export]
	Label CandyLabel;
	[Export]
	Label BestScoreLabel;
	[Export]
	Label GamesPlayedLabel;


    public override void _Ready()
    {
		Bird.Died += OnBirdDied;
		ReplayButton.Pressed += OnReplayButtonPressed;

        CandyLabel.SetText(GameData.Instance.TotalCandiesCollected.ToString());
        BestScoreLabel.SetText("BEST SCORE: " + GameData.Instance.BestScore);
        GamesPlayedLabel.SetText("GAMES PLAYED: " + GameData.Instance.TotalGamesPlayed);
    }

    private void OnReplayButtonPressed()
    {
        GetTree().ReloadCurrentScene();

		GameData.Instance.TotalCandiesCollected += CandyManager.CandiesCollected;
		if(GameData.Instance.BestScore < ScoreManager.Score)
		{
			GameData.Instance.BestScore = ScoreManager.Score;
		}
		GameData.Instance.TotalGamesPlayed++;
    }


    private void OnBirdDied()
    {
        Show();
		SetModulate(new Color(1f, 1f, 1f, 0f));

		ScoreLabel.SetText(ScoreManager.Score + "\nPOINTS");
		CandyLabel.SetText(CandyManager.CandiesCollected.ToString());

		var tween = CreateTween();
		tween.TweenProperty(this, "modulate:a", 1f, 0.75f)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);
    }

}
