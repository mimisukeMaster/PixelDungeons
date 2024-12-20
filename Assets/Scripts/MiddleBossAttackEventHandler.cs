using UnityEngine;

public class GolemEventHandler : MonoBehaviour
{
    public Golem golem;

    public void OnAttackLand()
    {
        Debug.Log("AttackLand Event");
        golem.OnAttack();
    }

    public void OnMoveEnd()
    {
        Debug.Log("Move End Event");
        golem.OnMoveEnd();
    }
}