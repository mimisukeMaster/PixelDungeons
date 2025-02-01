using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;

public class CountDownManager : MonoBehaviour
{
    public SpawnManager spawnManager;
    public GameObject CountDownUI;

    private TMP_Text textComponent;

    public bool DebugMode = false;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();


        StartCoroutine(nameof(CountDown));
        if(DebugMode)Debug.Log("カウントダウンデバッグモード");
        SceneManager.sceneLoaded +=  OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        transform.parent.gameObject.SetActive(true);
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        if (DebugMode) yield return null;
        else
        {
            for (int i = 3; i > 0; i--)
            {
                textComponent.text = i.ToString();
                yield return new WaitForSeconds(1.0f);
            }
            textComponent.text = "Start!";
        }

        // カウントダウン後、敵生成
        if (spawnManager != null) spawnManager.SpawnEnemies();
        else PlayerController.isGaming = true;

        yield return new WaitForSeconds(1.0f);
        CountDownUI.SetActive(false);

    }
}
