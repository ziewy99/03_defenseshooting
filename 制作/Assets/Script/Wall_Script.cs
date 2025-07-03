using UnityEngine;

public class Wall_Script : MonoBehaviour, IDamageable
{
    public int maxHP = 300;

    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    // IDamageableの実装
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("壁がダメージを受けた！ 残りHP: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;
            DestroyWall();
        }
    }

    private void DestroyWall()
    {
        Debug.Log("壁が破壊されました！");
        Destroy(gameObject);
    }

    // 現在のHPを取得（もしUIで使いたい場合）
    public int GetCurrentHP()
    {
        return currentHP;
    }

    // 最大HPを取得
    public int GetMaxHP()
    {
        return maxHP;
    }
}
