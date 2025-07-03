using UnityEngine;
using UnityEngine.UI;

public class Wall_HPUI : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;

    private Wall_Script wall;

    void Start()
    {
        wall = GetComponentInParent<Wall_Script>();
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (wall != null && healthFillImage != null)
        {
            float fillAmount = (float)wall.GetCurrentHP() / wall.GetMaxHP();
            healthFillImage.fillAmount = fillAmount;
        }
    }
}
