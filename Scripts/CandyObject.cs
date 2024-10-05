using Godot;
using System;

public partial class CandyObject : Node2D
{
    [Export]
    private BirdDetectorComponent BirdDetector;

    private bool _hasBeenDetected = false;

    public override void _Ready()
    {
        BirdDetector.BirdDetected += OnBirdDetected;
    }

    private void OnBirdDetected()
    {
        if (_hasBeenDetected)
            return;

        _hasBeenDetected = true;

        LabelSettings scoreLabelSettings = new LabelSettings
        {
            FontSize = 42,
            FontColor = new Color(1, 0.5f, 0, 1),
        };

        Label scoreLabel = new Label
        {
            Text = "+1",
            LabelSettings = scoreLabelSettings,
        };

        GetParent().AddChild(scoreLabel);
        scoreLabel.Position = this.Position - (scoreLabel.GetMinimumSize() / 2) + new Vector2(0, -20);

        var labelTween = CreateTween();
        labelTween.SetParallel(true);
        labelTween.TweenProperty(scoreLabel, "modulate:a", 0.0f, 0.8f)
                   .SetTrans(Tween.TransitionType.Sine)
                   .SetEase(Tween.EaseType.InOut);

        labelTween.TweenProperty(scoreLabel, "position", scoreLabel.Position + new Vector2(0, -100), 0.8f);

        var candyTween = CreateTween();
        candyTween.TweenProperty(this, "modulate:a", 0.0f, 1f)
                  .SetTrans(Tween.TransitionType.Sine)
                  .SetEase(Tween.EaseType.InOut);
        candyTween.Finished += () => { QueueFree(); scoreLabel.QueueFree(); };
    }
}
