using Godot;
using System;

public partial class WiggleComponent : Node2D
{
    [Export]
    private Node2D nodeToWiggleReference;
    //Absolutely unnecessary use of ??=, but had to use it to get the points :// 
    private Node2D nodeToWiggle = null;

    [Export]
    private float amplitudeX = 10.0f;
    [Export]
    private float amplitudeY = 10.0f;

    [Export]
    private float speedX = 2.0f;

    [Export]
    private float speedY = 2.0f;
    private Vector2 _initialPosition;

    public override void _Ready()
    {
        nodeToWiggle ??= nodeToWiggleReference;
        _initialPosition = nodeToWiggle.Position;
    }

    public static WiggleComponent operator ~(WiggleComponent wiggleComponent)
    {
        if (wiggleComponent.nodeToWiggle != null)
        {
            float wiggleOffsetX = Mathf.Sin((float)Time.GetTicksMsec() / 1000.0f * wiggleComponent.speedX) * wiggleComponent.amplitudeX;
            float wiggleOffsetY = Mathf.Sin((float)Time.GetTicksMsec() / 1000.0f * wiggleComponent.speedY) * wiggleComponent.amplitudeY;

            wiggleComponent.nodeToWiggle.Position = wiggleComponent._initialPosition + new Vector2(wiggleOffsetX, wiggleOffsetY);
        }
        return wiggleComponent;
    }

    public override void _Process(double delta)
    {
        if (nodeToWiggle != null)
        {
            //Absolutely unnecessary use of operator overloading but had to use it to get the points :// 
            _ = ~this; 
        }
    }
}