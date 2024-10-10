using Godot;
using System;

public partial class WiggleComponent : Node2D
{
    [Export]
    private Node2D nodeToWiggleReference;
    //Absolutely unnecessary use of ??=, but had to use it to get the points :// 
    private Node2D nodeToWiggle = null;

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
        nodeToWiggle ??= nodeToWiggleReference;
        _initialPosition = nodeToWiggle.Position; // Store the starting position
    }

    public static WiggleComponent operator ~(WiggleComponent wiggleComponent)
    {
        if (wiggleComponent.nodeToWiggle != null)
        {
            // Calculate wiggle effect based on sine wave and time
            float wiggleOffsetX = Mathf.Sin((float)Time.GetTicksMsec() / 1000.0f * wiggleComponent.speedX) * wiggleComponent.amplitudeX;
            float wiggleOffsetY = Mathf.Sin((float)Time.GetTicksMsec() / 1000.0f * wiggleComponent.speedY) * wiggleComponent.amplitudeY;

            // Apply the wiggle to the node's position
            wiggleComponent.nodeToWiggle.Position = wiggleComponent._initialPosition + new Vector2(wiggleOffsetX, wiggleOffsetY);
        }
        return wiggleComponent;
    }

    public override void _Process(double delta)
    {
        if (nodeToWiggle != null)
        {
            _ = ~this;
        }
    }
}