using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Button_UI : MonoBehaviour
{
    [SerializeField] private Coin_Controller coinController;
    [SerializeField] private TextMeshProUGUI RepairTower_Text;
    [SerializeField] private TextMeshProUGUI UpgradeArrow_Text;
    [SerializeField] private TextMeshProUGUI UpgradeATKSpeed_Text;

    [SerializeField] private TextMeshProUGUI warningText;
    private float warningDuration = 2f;
    private float warningTimer = 0f;

    [SerializeField] private string TipText_RepairTower;
    [SerializeField] private string TipText_UpgradeArrow;
    [SerializeField] private string TipText_UpgradeATKSpeed;
    [SerializeField] private TextMeshProUGUI tooltipTextUI;

    void Start()
    {
        if (coinController != null)
        {
            // Coin_Controllerからコストを取得してテキストに表示
            RepairTower_Text.text = $"Coin:{coinController.repairCost}";
            UpgradeArrow_Text.text = $"Coin:{coinController.upgradeArrowCost}";
            UpgradeATKSpeed_Text.text = $"Coin:{coinController.upgradeSpeedCost}";
        }
        else
        {
            Debug.LogWarning("Coin_Controllerが設定されていません！");
        }

        TipText_RepairTower = "Tower HP +400";
        TipText_UpgradeArrow = "Arrow Damage +10";
        TipText_UpgradeATKSpeed = "Attack Speed +0.2";
    }

    void Update()
    {
        if (coinController != null)
        {
            // Coin_Controllerからコストを取得してテキストに表示
            RepairTower_Text.text = $"Coin:{coinController.repairCost}";
            UpgradeArrow_Text.text = $"Coin:{coinController.upgradeArrowCost}";
            UpgradeATKSpeed_Text.text = $"Coin:{coinController.upgradeSpeedCost}";
        }
        else
        {
            Debug.LogWarning("Coin_Controllerが設定されていません！");
        }
        // 警告テキストの表示タイマー
        if (warningText != null && warningText.gameObject.activeSelf)
        {
            warningTimer += Time.deltaTime;
            if (warningTimer >= warningDuration)
            {
                warningText.text = "Right-click to spend 50 gold and create a wall.";
                warningTimer = 0f;
            }
        }
    }
    public void OnPointerEnter_RepairTower()
    {
        if (tooltipTextUI != null)
        {
            tooltipTextUI.text = TipText_RepairTower;
            tooltipTextUI.gameObject.SetActive(true);
        }
    }
    public void OnPointerEnter_UpgradeArrow()
    {
        if (tooltipTextUI != null)
        {
            tooltipTextUI.text = TipText_UpgradeArrow;
            tooltipTextUI.gameObject.SetActive(true);
        }
    }
    public void OnPointerEnter_UpgradeATKSpeed()
    {
        if (tooltipTextUI != null)
        {
            tooltipTextUI.text = TipText_UpgradeATKSpeed;
            tooltipTextUI.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit()
    {
        if (tooltipTextUI != null)
        {
            tooltipTextUI.text = "";
            tooltipTextUI.gameObject.SetActive(false);
        }
    }

    public void ShowWarning(string message)
    {
        if (warningText != null)
        {
            warningText.text = message;
            warningTimer = 0f;
        }
    }
}
