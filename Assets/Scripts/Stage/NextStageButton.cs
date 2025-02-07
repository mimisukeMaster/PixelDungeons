using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    public void NextStagePress()
    {
        // ステージをロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }

    public void HomePress()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;

    }
}
