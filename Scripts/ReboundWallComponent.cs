using Godot;
using System;

public partial class ReboundWallComponent : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Bird bird)
        {
            bird.OnHitWall();
        }
    }
}
