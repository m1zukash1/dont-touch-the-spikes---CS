using Godot;
using System;
using System.Threading.Tasks;

public partial class MenuController : Control
{
    [ExportGroup("Node References")]
    [Export]
    private Bird Bird;
    [Export]
    private GameManager GameManager;
    [Export]
    private Label CandyLabel;
    [Export]
    private Label BestScoreLabel;
    [Export]
    private Label GamesPlayedLabel;



    [ExportGroup("Animation varaibles")]    
    [Export]
    private float moveDuration = 0.825f;

    [Export]
    private float yOffset = 57.5f;


    [Signal]
    public delegate void GameStartedEventHandler();



    private Tween idleTween;
    private Tween startTween;
    private bool isIdle = false;
    private bool isGameStarted = false;

    public override void _Ready()
    {
        idleTween = CreateTween().SetLoops();
        StartIdleAnimation();

        //TODO: MAYBE FORMAT INTO SEPERATE FUNC LATER
        CandyLabel.SetText(GameData.Instance.TotalCandiesCollected.ToString());
        BestScoreLabel.SetText("BEST SCORE: " + GameData.Instance.BestScore);
        GamesPlayedLabel.SetText("GAMES PLAYED: " + GameData.Instance.TotalGamesPlayed);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent && touchEvent.IsPressed())
        {
            if (!isGameStarted)
            {
                StartGame();
            }
        }
    }

    private async void StartIdleAnimation()
    {
        isIdle = true;
        idleTween.Stop();
        Vector2 startPosition = Bird.Position;

        // Tweening upwards (jump animation)
        startTween = CreateTween();
        startTween.TweenProperty(Bird, "position:y", startPosition.Y + yOffset, moveDuration / 2f)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut)
            .Finished += () => Bird.AnimatedSprite2D.Play("jump"); // Play jump animation on upward tween

        // Play jump animation when moving upwards
        Bird.AnimatedSprite2D.Play("default");

        await ToSignal(startTween, Tween.SignalName.Finished);

        idleTween.Play();

        // Tweening downwards (default animation)
        idleTween.TweenProperty(Bird, "position:y", startPosition.Y - yOffset, moveDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut)
            .Finished += () => Bird.AnimatedSprite2D.Play("default"); // Play default animation on downward tween

        idleTween.TweenProperty(Bird, "position:y", startPosition.Y + yOffset, moveDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut)
            .Finished += () => Bird.AnimatedSprite2D.Play("jump"); // Play jump animation on upward tween
    }

    public void StopIdleAnimation()
    {
        isIdle = false;
        idleTween.Stop();
        startTween.Stop();
    }

    private async void StartGame()
    {
        EmitSignal(SignalName.GameStarted);
        isGameStarted = true;
        StopIdleAnimation();

        var fadeTween = CreateTween();

        fadeTween.TweenProperty(this, "modulate:a", 0f, 0.5f); 

        await ToSignal(fadeTween, Tween.SignalName.Finished);
        ProcessMode = ProcessModeEnum.Disabled;
        Hide();
    }
}
