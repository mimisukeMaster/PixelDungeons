using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [Tooltip("BGM処理")]
    public AudioSource AudioSource;
    [Tooltip("ステージBGM")]
    public AudioClip StageBGM;
    [Tooltip("敵の種類と出現数を設定")]
    public List<EnemyProperty> Enemies;
    [Tooltip("敵の種類と数を設定")]
    [System.Serializable]
    public struct EnemyProperty
    {
        public GameObject enemy;
        public int count;
    };
    [Tooltip("生成される範囲")]
    public Vector2 SpawnArea;
    [Tooltip("ステージのボス")]
    public GameObject Boss;
    [Tooltip("ボスが生まれる場所")]
    public Vector3 BossSpawnPos;
    [Tooltip("ゴールゲート")]
    public GameObject GoalGate;
    [HideInInspector]
    public static List<GameObject> EnemiesInStage = new List<GameObject>();

    private bool bossMode;
    private GameObject goalGate;


    private void Awake()
    {
        AudioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        AudioSource.volume = PlayerPrefs.GetFloat("SoundsValue", 1.0f);
        if (StageBGM)
        {
            AudioSource.clip = StageBGM;
            AudioSource.Play();
        }
        else Debug.LogWarning("ステージBGM未設定");
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        EnemiesInStage.Clear();
        PlayerController.isGaming = false;
    }

    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        EnemiesInStage.Clear();
    }

    void Update()
    {
        if (EnemiesInStage.Count == 0 && PlayerController.isGaming)
        {
            // 敵をすべて倒したらボス出現
            if (!bossMode)
            {
                GameObject boss = Instantiate(Boss, BossSpawnPos, Quaternion.identity);
                EnemiesInStage.Add(boss);
                bossMode = true;
            }
            // ボスを倒したらゴールゲート出現
            else if (goalGate == null) goalGate = Instantiate(GoalGate, BossSpawnPos, Quaternion.identity);
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            for (int j = 0; j < Enemies[i].count; j++)
            {
                float x = Random.Range(SpawnArea.x / -2.0f, SpawnArea.x / 2.0f);
                float z = Random.Range(SpawnArea.y / -2.0f, SpawnArea.y / 2.0f);

                GameObject enemy = Instantiate(Enemies[i].enemy, new Vector3(x, 0, z), Enemies[i].enemy.transform.rotation);
                EnemiesInStage.Add(enemy);
            }
        }

        // ゲーム中フラグ有効化
        PlayerController.isGaming = true;
    }
}
