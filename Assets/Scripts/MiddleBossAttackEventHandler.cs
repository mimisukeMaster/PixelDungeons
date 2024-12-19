using UnityEngine;

public class MiddleBossAttackEventHandler : MonoBehaviour
{
    private AttackController attackController;
    private MiddleBossEnemy middleBossEnemy;

    private void Start()
    {
        attackController = GetComponentInParent<AttackController>();
        middleBossEnemy = GetComponentInParent<MiddleBossEnemy>();
    }

    /// <summary>
    /// 攻撃イベント発生時の処理
    /// </summary>
    /// <remarks>アニメーションのタイミングでUnity側から呼ばれる</remarks>
    public void SmashEvent()
    {
        attackController.NearAttack(middleBossEnemy.SmashArea.position, middleBossEnemy.smashRange);
    }
}
