using UnityEngine;
using TMPro;

public class Game_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (gameManager != null)
        {
            coinText.text = $"Gold: {gameManager.GetGold()}";
            scoreText.text = $"Score: {gameManager.GetScore()}";
        }
    }
}
