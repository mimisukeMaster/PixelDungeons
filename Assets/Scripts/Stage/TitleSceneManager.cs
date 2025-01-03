using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject CreditsPanel;
    public GameObject SettingPanel;
    public GameObject Camera;

    public void OnStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        Camera.transform.Rotate(0f, 0.01f, 0f);
    }

    public void OnSetting()
    {
        SettingPanel.SetActive(!SettingPanel.activeSelf);
    }

    public void OnCredits()
    {
        CreditsPanel.SetActive(!CreditsPanel.activeSelf);
    }
}
