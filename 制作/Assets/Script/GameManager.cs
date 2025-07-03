using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currentTowerHP;
    public int towerMaxHP = 1000;

    public int arrowPower = 20;

    public int gold = 0;
    public int score = 0;

    [Header("Game Time Countdown")]
    public float gameTime = 120f;
    public float timer;
    public bool isGameOver = false;
    public bool isGameClear = false;

    [SerializeField] private TextMeshProUGUI countdownText;

    void Start()
    {
        currentTowerHP = towerMaxHP;
        timer = gameTime;
        UpdateCountdownText();
    }

    void Update()
    {
        if (isGameOver || isGameClear) return;

        timer -= Time.deltaTime;
        if (timer < 0f) timer = 0f;

        UpdateCountdownText();

        if (timer <= 0f)
        {
            GameClear();
        }
    }

    void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // タワーの現在HPを取得
    public int GetCurrentTowerHP()
    {
        return currentTowerHP;
    }

    // タワーの最大HPを取得
    public int GetMaxTowerHP()
    {
        return towerMaxHP;
    }

    // タワーのHPを減らす
    public void DamageTower(int damage)
    {
        if (isGameOver || isGameClear) return;

        currentTowerHP -= damage;
        if (currentTowerHP <= 0)
        {
            currentTowerHP = 0;
            isGameOver = true;

            ScoreManager.FinalScore = score;
            ScoreManager.IsGameOver = true;
            ScoreManager.IsGameClear = false;

            SceneManager.LoadScene("ResultScene");
        }
    }

    private void GameClear()
    {
        isGameClear = true;
        ScoreManager.FinalScore = score;
        ScoreManager.IsGameClear = true;
        ScoreManager.IsGameOver = false;
        SceneManager.LoadScene("ResultScene");
    }

    // タワーのHPを回復
    public void RepairTower(int amount)
    {
        currentTowerHP += amount;
        if (currentTowerHP > towerMaxHP)
        {
            currentTowerHP = towerMaxHP;
        }
        Debug.Log("タワーが修復された！ 現在HP: " + currentTowerHP);
    }

    // ゴールドを加算
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("ゴールドが増えた！ 現在のゴールド: " + gold);
    }

    // ゴールドを使用
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log("ゴールドを使用した！ 残り: " + gold);
            return true;
        }
        else
        {
            Debug.Log("ゴールドが足りない！");
            return false;
        }
    }

    public int GetGold()
    {
        return gold;
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("スコアが増えた！ 現在のスコア: " + score);
    }

    public int GetScore()
    {
        return score;
    }

    // 弓箭の攻撃力を取得
    public int GetArrowPower()
    {
        return arrowPower;
    }

    // 弓箭の攻撃力をアップグレード
    public void UpgradeArrowPower(int amount)
    {
        arrowPower += amount;
        Debug.Log("弓箭の攻撃力がアップ！ 現在: " + arrowPower);
    }
}
