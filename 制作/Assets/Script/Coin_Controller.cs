using UnityEngine;

public class Coin_Controller : MonoBehaviour
{
    public int repairCost = 20;
    public int upgradeArrowCost = 30;
    public int wallCost = 50;
    public int upgradeSpeedCost = 40;

    // コスト増加量
    public int repairCostIncrement = 20;
    public int upgradeArrowCostIncrement = 20;
    public int upgradeSpeedCostIncrement = 40;

    // 最大コスト設定
    public int maxRepairCost = 100;
    public int maxUpgradeArrowCost = 150;
    public int maxUpgradeSpeedCost = 400;

    public GameObject wallPrefab;
    public Archer_Script archerScript;

    [SerializeField] private Button_UI buttonUI;

    [Header("壁生成範囲")]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float fixedY = -3.0f;

    void Start()
    {
        if (buttonUI == null)
        {
            buttonUI = FindFirstObjectByType<Button_UI>();
        }
    }

    public void RepairTower()
    {
        var gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null && gameManager.SpendGold(repairCost))
        {
            gameManager.RepairTower(400);
            Debug.Log("タワーを修復しました！");
            repairCost = Mathf.Min(repairCost + repairCostIncrement, maxRepairCost);
        }
        else
        {
            buttonUI?.ShowWarning("Not enough gold!");
            Debug.Log("ゴールドが足りません！");
        }
    }

    public void UpgradeArrow()
    {
        var gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null && gameManager.SpendGold(upgradeArrowCost))
        {
            gameManager.UpgradeArrowPower(10);
            Debug.Log("弓矢の攻撃力がアップしました！");
            upgradeArrowCost = Mathf.Min(upgradeArrowCost + upgradeArrowCostIncrement, maxUpgradeArrowCost);
        }
        else
        {
            buttonUI?.ShowWarning("Not enough gold!");
            Debug.Log("ゴールドが足りません！");
        }
    }

    public void UpgradeAttackSpeed()
    {
        var gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null && gameManager.SpendGold(upgradeSpeedCost))
        {
            if (archerScript != null)
            {
                archerScript.IncreaseAttackSpeed(0.2f);
                Debug.Log("射撃速度がアップしました！");
                upgradeSpeedCost = Mathf.Min(upgradeSpeedCost + upgradeSpeedCostIncrement, maxUpgradeSpeedCost);
            }
        }
        else
        {
            buttonUI?.ShowWarning("Not enough gold!");
            Debug.Log("ゴールドが足りません！");
        }
    }

    public void PlaceWall()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;
        mouseWorldPosition.y = fixedY;

        var gameManager = FindFirstObjectByType<GameManager>();

        if (mouseWorldPosition.x >= minX && mouseWorldPosition.x <= maxX)
        {
            if (wallPrefab != null)
            {
                if (gameManager != null && gameManager.SpendGold(wallCost))
                {
                    Instantiate(wallPrefab, mouseWorldPosition, Quaternion.identity);
                    Debug.Log("壁を設置しました！");
                }
                else
                {
                    buttonUI?.ShowWarning("Not enough gold!");
                    Debug.Log("ゴールドが足りません！");
                }
            }
        }
        else
        {
            buttonUI?.ShowWarning("You can't place a wall outside the allowed area.");
            Debug.Log("壁を設置できる範囲外です！");
        }
    }
}
