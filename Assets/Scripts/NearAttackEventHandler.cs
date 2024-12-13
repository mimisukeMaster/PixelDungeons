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
    public void AttackEvent()
    {
        attackController.NearAttack(transform.parent.position, nearAttackEnemy.AttackDistance);

    }
}
