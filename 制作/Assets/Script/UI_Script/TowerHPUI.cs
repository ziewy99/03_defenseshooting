using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        UpdateTowerHealthUI();
    }

    void Update()
    {
        if (gameManager != null)
        {
            UpdateTowerHealthUI();
        }
    }

    private void UpdateTowerHealthUI()
    {
        int currentHP = gameManager.GetCurrentTowerHP();
        int maxHP = gameManager.GetMaxTowerHP();

        healthSlider.maxValue = maxHP;
        healthSlider.value = currentHP;

        healthText.text = $"{currentHP} / {maxHP}";
    }
}
