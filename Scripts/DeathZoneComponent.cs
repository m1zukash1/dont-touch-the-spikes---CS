using Godot;
using System;

public partial class DeathZoneComponent : Area2D
{
	[Export]
	private bool JumpOnCollide = false;
	[Export]
	private bool ResetVerticalVelocity = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is not Bird bird)
		{
			return;
		}
		bird.OnDied();
		if (JumpOnCollide)
		{
			bird.RequestJump();
		}
		if (ResetVerticalVelocity)
		{
			bird.Velocity = new Vector2(bird.Velocity.X, 0f);
		}
	}
}
