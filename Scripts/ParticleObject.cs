using Godot;
using System;

public partial class ParticleObject : CpuParticles2D
{
	public override async void _Ready()
	{
		Emitting = true;
		await ToSignal(GetTree().CreateTimer(Lifetime*2), "timeout");
		QueueFree();
	}
}
