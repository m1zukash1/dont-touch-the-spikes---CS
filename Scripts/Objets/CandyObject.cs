using Godot;
using System;

public partial class CandyObject : Node2D
{
    [Export]
    private BirdDetectorComponent BirdDetector;
    [Export]
    private Font LabelFont;


    [Signal]
    public delegate void CandyCollectedEventHandler();
    private bool _hasBeenDetected = false;

    private Tween startingTween;
    //Absolutely unnecessary use of constructor, but had to use it to get the points :// 
    public CandyObject()
    {
        startingTween = CreateTween();
    }
    ~CandyObject()
    {
        GD.Print("Object Deleted");   
    }
    public override void _Ready()
    {
        BirdDetector.BirdDetected += OnBirdDetected;
        
        Scale = new Vector2(0f, 0f);
        startingTween.TweenProperty(this, "scale", new Vector2(1f, 1f), 0.5f)
                   .SetTrans(Tween.TransitionType.Sine)
                   .SetEase(Tween.EaseType.InOut);
    }

    private void OnBirdDetected()
    {
        if (_hasBeenDetected)
        {
            return;
        }

        _hasBeenDetected = true;
        EmitSignal(SignalName.CandyCollected);

        LabelSettings scoreLabelSettings = new LabelSettings
        {
            FontSize = 64,
            FontColor = new Color("ff8207"),
            Font = LabelFont,
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

        labelTween.TweenProperty(scoreLabel, "position", scoreLabel.Position + new Vector2(0, -150), 0.8f);

        var candyTween = CreateTween();
        candyTween.TweenProperty(this, "modulate:a", 0.0f, 1f)
                  .SetTrans(Tween.TransitionType.Sine)
                  .SetEase(Tween.EaseType.InOut);
                  candyTween.Finished += () => { QueueFree(); scoreLabel.QueueFree(); };
    }
}
