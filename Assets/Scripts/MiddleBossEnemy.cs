using UnityEngine;

public class MiddleBossEnemy : EnemyController
{


    protected override void Update()
    {
        // 一定時間ごとにプレイヤー検知
        DetectPlayer();

        if (isChasing)
        {
            // ボスの攻撃モーション

            // 攻撃処理
        }
    }
}
