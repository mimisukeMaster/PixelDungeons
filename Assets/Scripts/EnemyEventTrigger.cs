using UnityEngine;

public class EnemyEventTrigger : MonoBehaviour
{
    public EnemyController EnemyController;

    public void AttackEvent()
    {
        Debug.Log("Attack");
    }
}
