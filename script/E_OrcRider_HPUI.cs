using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class E_OrcRider_HPUI : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;

    private E_OrcRider_Script enemy_orcrider;

    void Start()
    {
        enemy_orcrider = GetComponentInParent<E_OrcRider_Script>();
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (enemy_orcrider != null)
        {
            float fillAmount = (float)enemy_orcrider.currentHP / enemy_orcrider.maxHP;
            healthFillImage.fillAmount = fillAmount;
        }
    }
}
