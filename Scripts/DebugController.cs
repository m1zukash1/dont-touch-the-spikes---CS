using Godot;
using System;

public partial class DebugController : Control
{
	[Export]
	private bool ShowDebugInfo = false;

	[ExportGroup("Node References")]
	[Export]
	private Bird Bird;
	
	[Export]
	private Label DebugLabel;

	[Export]
	private int PrintsPerSecond = 32;

	private float timeElapsed = 0f;
	private float printInterval;

	public override void _Ready()
	{
		if(!ShowDebugInfo)
		{
			return;
		}
		printInterval = 1f / PrintsPerSecond;
		Show();
	}

    public override void _PhysicsProcess(double delta)
	{
		if(!ShowDebugInfo)
		{
			return;
		}
		// Increment the time elapsed by delta (time since last frame)
		timeElapsed += (float)delta;

		// If the time elapsed is greater than or equal to the interval, print the debug info
		if (timeElapsed >= printInterval)
		{
			DebugLabel.Text = Bird.ToString();
			timeElapsed = 0f;  // Reset the timer
		}
	}
}
