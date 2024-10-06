using DontTouchTheSpikes.Extensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SpikeManager : Node2D
{
    [ExportGroup("Object References")]
    [Export]
    private PackedScene SpikeObjectScene;
    [ExportGroup("Node References")]
    [Export]
    private SpikeSpawner spikeSpawner;
    [Export]
    private Bird Bird;
    [Export]
    private ScoreHandler ScoreHandler;

    private List<SpikeObject> leftSpikes;
    private List<SpikeObject> rightSpikes;
    
    private Random random = new Random();

    public override void _Ready()
    {
        spikeSpawner.SpawnSpikes();
        leftSpikes = spikeSpawner.LeftSpikes;
        rightSpikes = spikeSpawner.RightSpikes;
        
        //LoadSpikes();

		Bird.HitWall += OnBirdHitWall;
    }

    private void OnBirdHitWall()
    {
        LoadSpikes();
    }

    private void LoadSpikes()
    {
        DisableAllSpikes();

        List<SpikeObject> selectedSpikes;
        int numberOfSpikes = GetSpikeCountBasedOnScore();

        if (Bird.CurrentFacingDirection == Bird.FacingDirection.Right)
        {
            rightSpikes.Shuffle();
            selectedSpikes = rightSpikes.Take(numberOfSpikes).ToList();
        }
        else
        {
            leftSpikes.Shuffle();
            selectedSpikes = leftSpikes.Take(numberOfSpikes).ToList();
        }

        selectedSpikes.ForEach(spike => spike.Enable());
    }

    private void DisableAllSpikes()
    {
        leftSpikes.ForEach(spike => spike.Disable());
        rightSpikes.ForEach(spike => spike.Disable());
    }

private int GetSpikeCountBasedOnScore()
{
    int score = ScoreHandler.Score;
    bool isHardMode = false; // PLACE HOLDER

    (int minSpikes, int maxSpikes) = score switch
    {
        >= 50 when isHardMode => (8, 9),  // More spikes in hard mode
        >= 50 => (7, 8),
        >= 40 when isHardMode => (7, 8),
        >= 40 => (7, 7),
        >= 35 when isHardMode => (6, 8),
        >= 35 => (6, 7),
        >= 30 when isHardMode => (5, 8),
        >= 30 => (5, 7),
        >= 25 when isHardMode => (4, 8),
        >= 25 => (4, 7),
        >= 20 when isHardMode => (4, 7),
        >= 20 => (4, 6),
        >= 10 when isHardMode => (4, 6),
        >= 10 => (4, 5),
        >= 5 when isHardMode => (3, 5),
        >= 5 => (3, 4),
        _ => (2, 3)
    };

    return random.Next(minSpikes, maxSpikes + 1);
}

}
