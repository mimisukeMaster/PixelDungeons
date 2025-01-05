using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Tooltip("BGM処理")]
    public AudioSource AudioSource;
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
    public bool BossMode;
    [HideInInspector]
    public int existEnemyNum;


    private void Awake()
    {
        existEnemyNum = 0;
        AudioSource.volume = PlayerPrefs.GetFloat("SoundsValue", 1.0f);
        
        SpawnEnemies();
    }

    void Update()
    {
        if (existEnemyNum <= 0)
        {
            // 敵をすべて倒したらボス出現
            if (!BossMode)
            {
                Instantiate(Boss, BossSpawnPos, Quaternion.identity);
                existEnemyNum += 1;
                BossMode = true;
            }
            // ボスを倒したらゴールゲート出現
            else Instantiate(GoalGate, BossSpawnPos, Quaternion.identity);
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            for (int j = 0; j < Enemies[i].count; j++)
            {
                float x = Random.Range(SpawnArea.x / -2.0f, SpawnArea.x / 2.0f);
                float z = Random.Range(SpawnArea.y / -2.0f, SpawnArea.y / 2.0f);

                Instantiate(Enemies[i].enemy, new Vector3(x, 0, z), Enemies[i].enemy.transform.rotation);
                existEnemyNum += 1;
            }
        }
    }
}
