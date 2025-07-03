using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class E_EliteOrc_HPUI : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;

    private E_EliteOrc_Script enemy_eliteorc;

    void Start()
    {
        enemy_eliteorc = GetComponentInParent<E_EliteOrc_Script>();
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (enemy_eliteorc != null)
        {
            float fillAmount = (float)enemy_eliteorc.currentHP / enemy_eliteorc.maxHP;
            healthFillImage.fillAmount = fillAmount;
        }
    }
}
