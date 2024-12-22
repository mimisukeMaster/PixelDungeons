using UnityEngine;

public class NearAttackEventHandler : MonoBehaviour
{
    private AttackController attackController;
    private NearAttackEnemy nearAttackEnemy;

    private void Start()
    {
        attackController = GetComponentInParent<AttackController>();
        nearAttackEnemy = GetComponentInParent<NearAttackEnemy>();
    }

    /// <summary>
    /// 近距離敵の攻撃イベント発生時の処理
    /// </summary>
    /// <remarks>アニメーションのタイミングでUnity側から呼ばれる</remarks>
    public void AttackEvent()
    {
        attackController.NearAttack(transform.position, nearAttackEnemy.AttackDistance);

    }
}
