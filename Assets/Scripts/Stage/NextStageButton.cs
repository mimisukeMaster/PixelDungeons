using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    private int nowSceneIndex;

    private void Start()
    {
        nowSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void OnPress()
    {
        // 進行状況をセーブ
        PlayerPrefs.SetInt("PlayableStages", nowSceneIndex + 1);

        // ステージをロード
        SceneManager.LoadScene(nowSceneIndex + 1);
        Time.timeScale = 1.0f;
    }

    public void OnTitle()
    {
        // 進行状況をセーブ
        PlayerPrefs.SetInt("PlayableStages", nowSceneIndex + 1);

        // タイトルをロード
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
}
