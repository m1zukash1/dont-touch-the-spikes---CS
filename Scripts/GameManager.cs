using Godot;
using System;

public partial class GameManager : Node2D
{
	[Export]
	private Bird Bird;
	[Export]
	private MenuController MenuController;

    public override void _Ready()
    {
		MenuController.GameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
        Bird.CanMove = true;
    }
}
