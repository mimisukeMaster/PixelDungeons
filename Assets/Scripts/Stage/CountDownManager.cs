using UnityEngine;
using TMPro;
using System.Collections;

public class CountDownManager : MonoBehaviour
{
    public SpawnManager spawnManager;
    public GameObject CountDownUI;

    private TMP_Text textComponent;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();

        StartCoroutine(nameof(CountDown));
    }

    private IEnumerator CountDown()
    {
        for (int i = 3; i > 0; i--)
        {
            textComponent.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        textComponent.text = "Start!";

        // カウントダウン後、敵生成
        spawnManager.SpawnEnemies();

        yield return new WaitForSeconds(1.0f);
        CountDownUI.SetActive(false);

    }
}
