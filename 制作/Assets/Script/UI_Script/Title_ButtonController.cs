using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_ButtonController : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    // �uExit�v�{�^�������������ɌĂ΂��
    public void OnExitButton()
    {
        Debug.Log("�Q�[�����I�����܂��B");
        Application.Quit();
    }
}
