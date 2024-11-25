using UnityEngine;

/// <summary>
/// 遠距離攻撃の敵の処理
/// </summary>
/// <remarks>遠距離攻撃は遠方から弾を出す</remarks>
public class FarAttackEnemy : EnemyController
{
    public float AttackDistance = 3.0f;
    private void Update() {
        // 追跡中で一定距離以内になった場合、弾を出す処理
    }
}
