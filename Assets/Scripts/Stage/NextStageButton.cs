using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    public string NextSceneName;
    public void OnPress()
    {
        // 次のステージをロード
        SceneManager.LoadScene(NextSceneName);
        Time.timeScale = 1.0f;
    }
}
