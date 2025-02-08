using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルシーンの外観とボタンUI制御
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    public GameObject Camera;
    [Header("音")]
    public AudioSource AudioSource;
    public AudioClip TitleGingle;
    public AudioClip TitleBGM;
    public AudioClip ButtonSE;
    [Header("UI")]
    public GameObject StageSelectUI;
    public GameObject CreditsPanel;
    public GameObject SettingPanel;
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

        // ジングル再生
        AudioSource.clip = TitleGingle;
        AudioSource.loop = false;
        AudioSource.Play();
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

        // ジングルが終了したらBGMに変更
        if (!AudioSource.isPlaying)
        {
            AudioSource.clip = TitleBGM;
            AudioSource.loop = true;
            AudioSource.Play();
        }
    }

    /// <summary>
    /// スタートボタン
    /// </summary>
    public void OnStart()
    {
        AudioSource.PlayOneShot(ButtonSE);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// 設定ボタン
    /// </summary>
    public void OnSetting()
    {
        AudioSource.PlayOneShot(ButtonSE);
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
        AudioSource.PlayOneShot(ButtonSE);
        CreditsPanel.SetActive(!CreditsPanel.activeSelf);
    }
}
