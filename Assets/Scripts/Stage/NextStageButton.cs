using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    public void OnPress()
    {
        // ステージをロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }

    public void OnTitle()
    {
        // タイトルをロード
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
}
