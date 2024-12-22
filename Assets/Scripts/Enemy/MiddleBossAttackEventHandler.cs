using UnityEngine;

public class GolemEventHandler : MonoBehaviour
{
    public Golem golem;

    public void OnAttackLand()
    {
        golem.OnAttackLand();
    }

    public void OnAttackEnd()
    {
        golem.OnAttackEnd();
    }

    public void OnMoveEnd()
    {
        golem.OnMoveEnd();
    }

}