using UnityEngine;

public class Tower_Script : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.DamageTower(damage);
        }
        else
        {
            Debug.LogWarning("GameManager が見つかりませんでした。");
        }
    }

}
