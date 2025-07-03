using UnityEngine;

public class ESC_Controller : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("ÉQÅ[ÉÄÇèIóπÇµÇ‹Ç∑ÅB");
        }
    }
}
