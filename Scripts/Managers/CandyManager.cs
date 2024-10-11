using Godot;
using System;

public partial class CandyManager : Node2D
{
	[Export]
	private Bird Bird;
	[Export]
	private PackedScene CandyObjectScene;

	private int MinVerticalRange = 152;
	private int MaxVerticalRange = 1104;

	private Vector2 leftSidePosition = new Vector2(104, 0);
	private Vector2 rightSidePosition = new Vector2(616, 0);

	public int CandiesCollected = 0;

	private bool spawnOnLeft = true;

	public override void _Ready()
	{
		Bird.HitWall += OnBirdHitWall;
	}

	private bool firstCandySpawned = false;
	private void OnBirdHitWall()
	{
		if(firstCandySpawned)
		{
			return;
		}
		firstCandySpawned = true;
		SpawnCandy();
	}

	private void SpawnCandy()
	{
		if(Bird.isDead)
		{
			return;
		}
		var candyObject = CandyObjectScene.Instantiate<CandyObject>();
		candyObject.CandyCollected += OnCandyCollected;

		float randomY = (float)GD.RandRange(MinVerticalRange, MaxVerticalRange);

		candyObject.Position = spawnOnLeft 
			? new Vector2(leftSidePosition.X, randomY) 
			: new Vector2(rightSidePosition.X, randomY);

		CallDeferred("add_child", candyObject); //SpawnCandy() is called in the middle of flushing queries so we deffer the next candy when safe to do.
	}
	private void OnCandyCollected()
	{
		spawnOnLeft = !spawnOnLeft;
		int increment = 1;
		while (increment != 0)
		{
			int carry = CandiesCollected & increment;             // Calculate the carry
			CandiesCollected = CandiesCollected ^ increment;      // XOR to add without the carry
			increment = carry << 1;                               // Shift the carry to the left
		}
		SpawnCandy();
	}
}
