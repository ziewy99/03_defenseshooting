using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class E_Orc_HPUI : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;

    private E_Orc_Script enemy_orc;

    void Start()
    {
        enemy_orc = GetComponentInParent<E_Orc_Script>();
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (enemy_orc != null)
        {
            float fillAmount = (float)enemy_orc.currentHP / enemy_orc.maxHP;
            healthFillImage.fillAmount = fillAmount;
        }
    }
}
