using UnityEngine;

public class WeaponEventHandler : MonoBehaviour
{
    private AttackController attackController;
    private PlayerController playerController;

    private void Start()
    {
        attackController = GetComponentInParent<AttackController>();
        playerController = GetComponentInParent<PlayerController>();
    }

    /// <summary>
    /// 武器の攻撃イベント発生時の処理
    /// </summary>
    /// <remarks>アニメーションのタイミングでUnity側から呼ばれる</remarks>
    /// 近遠両方で呼ばないといけなく、nullチェックを通して近距離攻撃を行う
    public void AttackEvent() => attackController?.NearAttack(transform.position, playerController.NearAttackRange);
}
