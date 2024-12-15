using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Tooltip("敵の種類と出現数を設定")]
    public Dictionary<GameObject, int> Enemies;
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
        for (int i = 0; i > Enemies.Count; i++)
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(SpawnArea.x / -2.0f, SpawnArea.x / 2.0f),
                Random.Range(SpawnArea.y / -2.0f, SpawnArea.y / 2.0f));
        }
    }
}
