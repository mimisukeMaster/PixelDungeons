using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour
{
    public void OnRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void OnTitle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1.0f;
    }
}
