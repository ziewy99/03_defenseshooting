using UnityEngine;
using TMPro;

public class Ending_ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    void Start()
    {
        if (ScoreManager.IsGameOver)
        {
            resultText.text = $"Game Over!\nScore: {ScoreManager.FinalScore}";
        }
        else if (ScoreManager.IsGameClear)
        {
            resultText.text = $"Game Clear!\nScore: {ScoreManager.FinalScore}";
        }

        ScoreManager.Reset(); // データリセット
    }
}
