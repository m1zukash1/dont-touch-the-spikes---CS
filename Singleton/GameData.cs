using Godot;
using System;

public partial class GameData : Node
{
    public static GameData Instance { get; private set; }


    public int TotalCandiesCollected;
    public int TotalGamesPlayed;
    public int BestScore;

    public override void _Ready()
    {
        Instance = this;
        TotalCandiesCollected = 0;
        TotalGamesPlayed = 0;
        BestScore = 0;
    }
}
