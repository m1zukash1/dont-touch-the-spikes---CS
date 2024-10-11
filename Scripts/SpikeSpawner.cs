using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SpikeSpawner : Node
{
    private const int NumberOfSpikes = 11;
    private const int SpikeSpacing = 88;
    private readonly int InitialYPosition = 184;
    private readonly float LeftSideXPosition = 0 - 48;
    private readonly float RightSideXPosition = 720 + 48;

    [ExportGroup("Object References")]
    [Export]
    private PackedScene SpikeObjectScene;

    public readonly List<SpikeObject> LeftSpikes = new List<SpikeObject>();
    public readonly List<SpikeObject> RightSpikes = new List<SpikeObject>();

    public void SpawnSpikes()
    {
        foreach (var yPos in GenerateSpikePositions())
        {
            var leftSpike = SpawnSpike(LeftSideXPosition, yPos);
            LeftSpikes.Add(leftSpike);

            var rightSpike = SpawnSpike(RightSideXPosition, yPos, flipX: true);
            RightSpikes.Add(rightSpike);
        }
    }
    private IEnumerable<int> GenerateSpikePositions()
    {
        return Enumerable.Range(0, NumberOfSpikes)
                         .Select(i => InitialYPosition + i * SpikeSpacing);
    }
    private SpikeObject SpawnSpike(float xPos, int yPos, bool flipX = false)
    {
        var spikeObject = SpikeObjectScene.Instantiate<SpikeObject>();
        spikeObject.Position = new Vector2(xPos, yPos);

        if (flipX)
        {
            spikeObject.Scale = new Vector2(-1, spikeObject.Scale.Y);
        }

        GetParent().AddChild(spikeObject);
        return spikeObject;
    }
}
