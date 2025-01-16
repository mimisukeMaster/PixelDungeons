using UnityEngine;

public class NearAttackEventHandler : MonoBehaviour
{
    private AttackController attackController;
    private NearAttackEnemy nearAttackEnemy;
    public GameObject attackPrefab;

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
        GameObject attack = Instantiate(attackPrefab,transform.position,transform.rotation);
        attack.GetComponent<AttackController>().Init("Player",nearAttackEnemy.Attack,0.2f,1);
    }
}
