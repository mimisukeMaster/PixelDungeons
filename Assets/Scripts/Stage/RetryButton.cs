using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour
{
    private int nowSceneIndex;

    private void Start()
    {
        nowSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void OnRetry()
    {
        SceneManager.LoadScene(nowSceneIndex);
        Time.timeScale = 1.0f;
    }
}
