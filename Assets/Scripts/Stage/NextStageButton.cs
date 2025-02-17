using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    public AudioClip HomeBGM;
    public void NextStagePress()
    {
        // ステージをロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }

    public void HomePress()
    {
        AudioSource audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
        audioSource.clip = HomeBGM;
        audioSource.Play();
        
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }
}
