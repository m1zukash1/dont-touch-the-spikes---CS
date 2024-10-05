using Godot;
using System;

public partial class WiggleComponent : Node2D
{
    [Export]
    private Node2D nodeToWiggle;

    [Export]
    private float amplitudeX = 10.0f; // Amplitude for X-axis wiggle

    [Export]
    private float amplitudeY = 10.0f; // Amplitude for Y-axis wiggle

    [Export]
    private float speedX = 2.0f; // Speed for X-axis wiggle

    [Export]
    private float speedY = 2.0f; // Speed for Y-axis wiggle

    private Vector2 _initialPosition; // Store initial position

    public override void _Ready()
    {
        // Ensure we have a valid Node2D to wiggle
        if (nodeToWiggle == null)
        {
            GD.PushError("NodeToWiggle is not set in WiggleComponent.");
            return;
        }

        _initialPosition = nodeToWiggle.Position; // Store the starting position
    }

    public override void _Process(double delta)
    {
        if (nodeToWiggle != null)
        {
            // Calculate wiggle effect based on sine wave and time
            float wiggleOffsetX = Mathf.Sin((float)Time.GetTicksMsec() / 1000.0f * speedX) * amplitudeX;
            float wiggleOffsetY = Mathf.Sin((float)Time.GetTicksMsec() / 1000.0f * speedY) * amplitudeY;

            // Apply the wiggle to the node's position
            nodeToWiggle.Position = _initialPosition + new Vector2(wiggleOffsetX, wiggleOffsetY);
        }
    }
}