using UnityEngine;

public class GolemEventHandler : MonoBehaviour
{
    private AttackController attackController;
    private Golem golem;
    private const int ARM_ANIMATION_SPAN = 5;

    
    private void Start()
    {
        attackController = GetComponentInParent<AttackController>();
        golem = GetComponentInParent<Golem>();
    }

    public void OnAttackLand()
    {
        attackController.NearAttack(golem.SmashPosition.position, golem.SmashRange);
    }

    public void OnAttackEnd() => golem.CheckNextMove();

    public void OnMoveEnd()
    {
        golem.transform.position += golem.transform.forward * ARM_ANIMATION_SPAN;
        golem.CheckNextMove();
    }

    public void OnIdleEnd() => golem.CheckNextMove();

}