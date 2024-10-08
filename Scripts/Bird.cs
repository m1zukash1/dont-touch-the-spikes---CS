using Godot;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks; // Import for async, delay, and cancellation

public partial class Bird : CharacterBody2D
{
    public enum FacingDirection
    {
        Left,
        Right
    }

    [ExportGroup("Object References")]
    [Export]
    private PackedScene ParticleObjectScene;  // PackedScene for particle effect

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

    // Particle cooldown variables
    private float particleCooldown = 0.33f; // Cooldown time for particles
    private bool particleCooldownExpired = true;

    // For managing jump task cancellation
    private CancellationTokenSource jumpCancellationTokenSource;

    public bool CanMove { get; set; } = false;
    
    public override void _Ready()
    {
        // Initialize anything necessary here
    }

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
        Debug.Assert(false); //IMPLEMENT DEATH!
        isDead = true;
        EmitSignal(SignalName.Died);
        speed *= 3;
        Velocity = new Vector2(speed, Velocity.Y);
        AnimatedSprite2D.Play("death");
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

        // Cancel the previous jump if it's still awaiting
        jumpCancellationTokenSource?.Cancel();
        
        // Create a new CancellationTokenSource for this jump
        jumpCancellationTokenSource = new CancellationTokenSource();
        
        Jump(jumpCancellationTokenSource.Token);
    }

    private async void Jump(CancellationToken cancellationToken)
    {
        Velocity = new Vector2(Velocity.X, -jumpSpeed);

        if (!isDead)
        {
            AnimatedSprite2D.Play("jump");

            // Only spawn particle if cooldown has expired
            if (particleCooldownExpired)
            {
                SpawnParticle();
            }

            try
            {
                // Await the delay, but cancel if requested
                await Task.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken);
                
                // If not canceled, play default animation
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
            particleCooldownExpired = false; // Set cooldown to prevent spawning more particles immediately

            CpuParticles2D particleInstance = ParticleObjectScene.Instantiate<CpuParticles2D>();
            AddChild(particleInstance);
            MoveChild(particleInstance, 0); // Move particle node to front

            // Wait for the cooldown duration before allowing new particles
            await Task.Delay(TimeSpan.FromSeconds(particleCooldown));

            particleCooldownExpired = true; // Cooldown finished, allow particle spawning again
        }
    }
}
