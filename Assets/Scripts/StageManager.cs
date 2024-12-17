using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Tooltip("敵の種類と出現数を設定")]
    public List<EnemyPropaty> Enemies;
    [Tooltip("敵の種類と数を設定")]
    [System.Serializable]
    public struct EnemyPropaty{
        public GameObject enemy;
        public int count;
    };
    [Tooltip("生成される範囲")]
    public Vector2 SpawnArea;

    private void Start()
    {
        SpawnEnemies();
    }

    void Update()
    {
        
    }

    private void SpawnEnemies()
    {
        for(int i = 0; i < Enemies.Count; i++)
        {
            for(int j = 0;j < Enemies[i].count; j++){
                float x = Random.Range(SpawnArea.x / -2.0f, SpawnArea.x / 2.0f);
                float z = Random.Range(SpawnArea.y / -2.0f, SpawnArea.y / 2.0f);

                Instantiate(Enemies[i].enemy, new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }
}
