using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending_ButtonController : MonoBehaviour
{
    public void OnRestartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackToTitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
