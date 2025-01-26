using UnityEngine;
using TMPro;
using System.Collections;
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
    }

    private IEnumerator CountDown()
    {
        for (int i = 3; i > 0; i--)
        {
            textComponent.text = i.ToString();
            if(DebugMode)yield return null;
            else yield return new WaitForSeconds(1.0f);
        }
        textComponent.text = "Start!";

        // カウントダウン後、敵生成
        if(spawnManager != null)spawnManager.SpawnEnemies();

        yield return new WaitForSeconds(1.0f);
        CountDownUI.SetActive(false);

    }
}
