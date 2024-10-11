using Godot;
using System;

public partial class BirdDetectorComponent : Area2D
{
	[Signal]
	public delegate void BirdDetectedEventHandler();
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

    private void OnBodyEntered(Node2D body)
    {
		if(body is not Bird Bird)
		{
			return;
		}
		if(Bird.isDead)
		{
			return;
		}
		EmitSignal(SignalName.BirdDetected);
    }
}
