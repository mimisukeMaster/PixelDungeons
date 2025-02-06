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

    public bool IsDebugMode;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        StartCoroutine(CountDown());
        if(IsDebugMode)Debug.Log("カウントダウンデバッグモード");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        transform.parent.gameObject.SetActive(true);
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        // 戦闘シーンのみカウントダウンして始まる
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            for (int i = 3; i > 0; i--)
            {
                textComponent.text = i.ToString();
                if(IsDebugMode)yield return null;
                else yield return new WaitForSeconds(1.0f);
            }
            textComponent.text = "Start!";
            if (spawnManager != null) spawnManager.SpawnEnemies();
        }
        
        PlayerController.isGaming = true;

        yield return new WaitForSeconds(1.0f);
        CountDownUI.SetActive(false);

    }
}
