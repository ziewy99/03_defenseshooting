using UnityEngine;
using UnityEngine.UI;

public class LongPress_UI : MonoBehaviour
{
    [SerializeField] private Archer_Script archerScript;
    [SerializeField] private Image longPressBar;
    [SerializeField] private Image barFullHint;

    void Update()
    {
        if (archerScript != null && longPressBar != null)
        {
            if (archerScript.isPressing)
            {
                // 長押し時間に応じてバーを伸ばす（0 → 1）
                float fillAmount = Mathf.Clamp01(archerScript.pressTime / archerScript.longPressThreshold);
                longPressBar.fillAmount = fillAmount;

                // 長押し時間が閾値を超えたらヒントを表示
                if (fillAmount >= 1f && barFullHint != null)
                {
                    barFullHint.enabled = true;
                }
                else if (barFullHint != null)
                {
                    barFullHint.enabled = false;
                }
            }
            else
            {
                // 離したらリセット
                longPressBar.fillAmount = 0f;
                if (barFullHint != null)
                {
                    barFullHint.enabled = false;
                }
            }
        }
    }
}
