using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_ButtonController : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    // 「Exit」ボタンを押した時に呼ばれる
    public void OnExitButton()
    {
        Debug.Log("ゲームを終了します。");
        Application.Quit();
    }
}
