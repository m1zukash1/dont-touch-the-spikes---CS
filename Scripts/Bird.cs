using Godot;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
public partial class Bird : CharacterBody2D, IFormattable
{
    public enum FacingDirection
    {
        Left,
        Right
    }

    [ExportGroup("Object References")]
    [Export]
    private PackedScene ParticleObjectScene;

    [ExportGroup("Node References")]
    [Export]
    public AnimatedSprite2D AnimatedSprite2D { get; set; }

    [Signal]
    public delegate void HitWallEventHandler();
    [Signal]
    public delegate void DiedEventHandler();

    private float speed = 400f;
    private float jumpSpeed = 700f;
    private float gravity = 2000f;
    public bool isDead = false;
    public FacingDirection CurrentFacingDirection { get; private set; } = FacingDirection.Right;

    [Export]
    private float wallHitCooldown = 0.5f;
    private bool canHitWall = true;
    private float particleCooldown = 0.33f;
    private bool particleCooldownExpired = true;
        private CancellationTokenSource jumpCancellationTokenSource;

    public bool CanMove { get; set; } = false;

    public async void OnHitWall()
    {
        if (!canHitWall)
        {
            return; 
        }

        canHitWall = false;

        speed = -speed;

        AnimatedSprite2D.FlipH = !AnimatedSprite2D.FlipH;

        CurrentFacingDirection = AnimatedSprite2D.FlipH ? FacingDirection.Left : FacingDirection.Right;

        Velocity = new Vector2(speed, Velocity.Y);
        EmitSignal(SignalName.HitWall);

        await Task.Delay(TimeSpan.FromSeconds(wallHitCooldown));

        canHitWall = true;
    }

    public void OnDied()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        EmitSignal(SignalName.Died);
        speed *= 3;
        Velocity = new Vector2(speed, Velocity.Y);
        AnimatedSprite2D.Play("death");

        var tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 0f, 0.75f)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut)
            .Finished += () => Hide();
    }
    public override void _Input(InputEvent @event)
    {
        if (isDead)
        {
            return;
        }
        if (@event is InputEventScreenTouch touchEvent && touchEvent.IsPressed())
        {
            RequestJump();
        }
    }
    public override void _Process(double delta)
    {
        if(!CanMove)
        {
            return;
        }

        Velocity = new Vector2(speed, (float)(Velocity.Y + gravity * delta));
        SetVelocity(Velocity);
        MoveAndSlide();

        if(isDead)
        {
            RotationDegrees += (float)(1000 * delta);
        }
    }
    public void RequestJump()
    {
        if (!CanMove && isDead)
        {
            return;
        }

        jumpCancellationTokenSource?.Cancel();
        
        jumpCancellationTokenSource = new CancellationTokenSource();
        
        Jump(jumpCancellationTokenSource.Token);
    }
    private async void Jump(CancellationToken cancellationToken)
    {
        Velocity = new Vector2(Velocity.X, -jumpSpeed);

        if (!isDead)
        {
            AnimatedSprite2D.Play("jump");

            if (particleCooldownExpired)
            {
                SpawnParticle();
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken);
                
                if(!isDead)
                {
                    AnimatedSprite2D.Play("default");
                }
                else
                {
                    AnimatedSprite2D.Play("death");
                }
            }
            catch (TaskCanceledException)
            {
                if(isDead)
                {
                    AnimatedSprite2D.Play("death");
                }
            }
        }
    }

    private async void SpawnParticle()
    {
        if (ParticleObjectScene != null && particleCooldownExpired)
        {
            particleCooldownExpired = false;

            CpuParticles2D particleInstance = ParticleObjectScene.Instantiate<CpuParticles2D>();
            AddChild(particleInstance);
            MoveChild(particleInstance, 0);

            await Task.Delay(TimeSpan.FromSeconds(particleCooldown));

            particleCooldownExpired = true;
        }
    }
    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";
        if (formatProvider == null) formatProvider = CultureInfo.CurrentCulture;

        switch (format.ToUpperInvariant())
        {
            case "G":
            case "FULL":
                return $"Position: ({GlobalPosition.X.ToString("F2", formatProvider)}, {GlobalPosition.Y.ToString("F2", formatProvider)})\n" +
                    $"Velocity: ({Velocity.X.ToString("F2", formatProvider)}, {Velocity.Y.ToString("F2", formatProvider)})\n" +
                    $"Current Animation: {AnimatedSprite2D.Animation}\n" +
                    $"Facing Direction: {CurrentFacingDirection}\n" +
                    $"Can Hit Wall: {canHitWall}\n" +
                    $"Is Dead: {isDead}\n" +
                    $"Particle Cooldown Expired: {particleCooldownExpired}\n" +
                    $"Can Move: {CanMove}";
            default:
                throw new FormatException($"The '{format}' format is not supported.");
        }
    }
    public override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }
}
