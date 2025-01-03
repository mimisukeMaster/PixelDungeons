using UnityEngine;

public class GolemEventHandler : MonoBehaviour
{
    public GameObject SmashWave;
    private AttackController attackController;
    private Golem golem;
    private const int ARM_ANIMATION_SPAN = 5;

    
    private void Start()
    {
        attackController = GetComponentInParent<AttackController>();
        golem = GetComponentInParent<Golem>();
        SmashWave.SetActive(false);
    }

    public void OnAttackLand()
    {
        golem.OnAttackLand();
        attackController.NearAttack(golem.SmashPosition.position, golem.SmashRange);
        SmashWave.SetActive(true);
    }

    public void OnAttackEnd()
    {
        golem.CheckNextMove();
        SmashWave.SetActive(false);
    }

    public void OnMoveEnd()
    {
        golem.transform.position += golem.transform.forward * ARM_ANIMATION_SPAN;
        golem.CheckNextMove();
    }

    public void OnBeamStart() => golem.OnBeamStart();//アニメーションが始まる直後に呼ぶ

    public void OnBeamEnd()=> golem.CheckNextMove();

    public void OnIdleEnd() => golem.CheckNextMove();
}