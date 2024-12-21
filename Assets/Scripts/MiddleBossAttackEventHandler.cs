using UnityEngine;

public class GolemEventHandler : MonoBehaviour
{
    public Golem golem;

    public void OnAttackLand()
    {
        golem.OnAttack();
    }

    public void OnMoveEnd()
    {
        golem.OnMoveEnd();
    }
}