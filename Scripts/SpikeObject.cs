using Godot;
using System;

public partial class SpikeObject : Node2D
{
    [Export]
    private DeathZoneComponent deathZoneComponent;

    public bool IsEnabled { get; private set; } = false;

    private const int Offset = 48;

    public void Enable()
    {
        if (IsEnabled)
        {
            throw new InvalidOperationException("Spike is already enabled.");
        }

        IsEnabled = true;

        float targetXPosition = Position.X + (Scale.X == -1 ? -Offset : Offset);

        Tween tween = CreateTween();
        tween.TweenProperty(this, "position:x", targetXPosition, 0.25);
        tween.Play();
    }

    public void Disable()
    {
        if (!IsEnabled)
        {
            return;
        }

        IsEnabled = false;

        float targetXPosition = Position.X + (Scale.X == -1 ? Offset : -Offset);

        Tween tween = CreateTween();
        tween.TweenProperty(this, "position:x", targetXPosition, 0.25);
        tween.Play();
    }
}
