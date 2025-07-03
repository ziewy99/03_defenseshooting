using UnityEngine;

public static class ScoreManager
{
    public static int FinalScore { get; set; }
    public static bool IsGameOver { get; set; }
    public static bool IsGameClear { get; set; }

    public static void Reset()
    {
        FinalScore = 0;
        IsGameOver = false;
        IsGameClear = false;
    }
}

