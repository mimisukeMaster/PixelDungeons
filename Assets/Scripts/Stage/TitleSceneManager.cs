using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルシーンの外観とボタンUI制御
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    public GameObject CreditsPanel;
    public GameObject SettingPanel;
    public GameObject Camera;
    public Animator BirdAnim;
    public Animator GolemAnim;

    private void Start()
    {
        // 鳥の挙動
        BirdAnim.SetBool("Chasing", true);
        BirdAnim.SetFloat("WingSpeed", 2.0f);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    /// <summary>
    /// 設定ボタン
    /// </summary>
    public void OnSetting()
    {
        SettingPanel.SetActive(!SettingPanel.activeSelf);
    }

    /// <summary>
    /// クレジットボタン
    /// </summary>
    public void OnCredits()
    {
        CreditsPanel.SetActive(!CreditsPanel.activeSelf);
    }
}
