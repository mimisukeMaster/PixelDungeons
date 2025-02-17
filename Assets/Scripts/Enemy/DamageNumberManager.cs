using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageNumberManager : MonoBehaviour
{
    [Tooltip("ダメージのUIの最大値")]
    public int MaxDamageUINumber = 30;
    private static int MaxDamageUINumver_S;
    [Tooltip("ダメージのUI")]
    public GameObject Panel;
    private static GameObject Panel_S;
    public GameObject Canvas;
    private static GameObject Canvas_S;
    [Tooltip("UIが表示される時間")]
    public float displayTime;
    private static GameObject[] UIs;
    private static int index;
    private WaitForSeconds waitForSeconds;
    private static DamageNumberManager damageNumberManager;

    private void Start()
    {
        Panel_S = Panel;
        Canvas_S = Canvas;
        MaxDamageUINumver_S = MaxDamageUINumber;
        damageNumberManager = this;
        UIs = new GameObject[MaxDamageUINumber];
        waitForSeconds = new WaitForSeconds(displayTime);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        Canvas_S = Canvas;
    }

    public static void AddUI(int damage,Vector3 target)
    {
        if(index >= MaxDamageUINumver_S)index = 0;
        GameObject UI;
        if(UIs[index] == null)UI = Instantiate(Panel_S,Canvas_S.transform);
        else
        {
            UI = UIs[index];
            UI.SetActive(true);
        }
        UI.GetComponent<DamageNumberUI>().Init(damage,target);
        index++;
        damageNumberManager.DisableUICount(UI);
    }

    private void DisableUICount(GameObject UI)
    {
        if (UI) StartCoroutine(DisableUI(UI));
    }

    private IEnumerator DisableUI(GameObject UI)
    {
        yield return waitForSeconds;
        UI.SetActive(false);
    }
}
