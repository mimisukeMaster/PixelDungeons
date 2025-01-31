using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// タイトルシーンの外観とボタンUI制御
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public GameObject Camera;
    [Header("UI")]
    public GameObject StageSelectUI;
    public GameObject CreditsPanel;
    public GameObject SettingPanel;
    // public List<Button> StageButtons;
    public Slider SoundsSlider;
    [Header("景観")]
    public Animator BirdAnim;
    public Animator GolemAnim;

    private void Start()
    {
        // UI非表示
        StageSelectUI.SetActive(false);
        CreditsPanel.SetActive(false);
        SettingPanel.SetActive(false);

        // 鳥の挙動
        BirdAnim.SetBool("Chasing", true);
        BirdAnim.SetFloat("WingSpeed", 2.0f);

        // シーン間共有の値を取得し制御
        AudioSource.volume = PlayerPrefs.GetFloat("SoundsValue", 1.0f);
        SoundsSlider.value = AudioSource.volume;
        
        // for (int i = 0; i < PlayerPrefs.GetInt("PlayableStages", 1); i++) StageButtons[i].interactable = true;
    }

    private void FixedUpdate()
    {
        // カメラの挙動
        Camera.transform.Rotate(0f, 0.04f, 0f, Space.World);

        // ゴーレムの挙動
        GolemAnim.SetTrigger("Attack");

        // ユーザ操作（隠し操作）
        if (Input.GetKey(KeyCode.A)) Camera.transform.Rotate(0f, -1.0f, 0f, Space.World);
        if (Input.GetKey(KeyCode.D)) Camera.transform.Rotate(0f, 1.0f, 0f, Space.World);
    }

    /// <summary>
    /// スタートボタン
    /// </summary>
    public void OnStart()
    {
        // StageSelectUI.SetActive(!StageSelectUI.activeSelf);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnStageGrass()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnStageSky()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void OnStageCastle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    /// <summary>
    /// 設定ボタン
    /// </summary>
    public void OnSetting()
    {
        SettingPanel.SetActive(!SettingPanel.activeSelf);
    }

    /// <summary>
    /// 音量変更ボタン
    /// </summary>
    public void OnSoundsChanged()
    {
        AudioSource.volume = SoundsSlider.value;

        // シーン間共有で値を保存
        PlayerPrefs.SetFloat("SoundsValue", SoundsSlider.value);
    }

    /// <summary>
    /// クレジットボタン
    /// </summary>
    public void OnCredits()
    {
        CreditsPanel.SetActive(!CreditsPanel.activeSelf);
    }
}
