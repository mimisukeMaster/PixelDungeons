using UnityEngine;

public class GolemEventHandler : MonoBehaviour
{
    public GameObject SmashWave;
    public GameObject attackPrefab;
    private Golem golem;
    private const int ARM_ANIMATION_SPAN = 5;

    
    private void Start()
    {
        golem = GetComponentInParent<Golem>();
        SmashWave.SetActive(false);
    }

    public void OnAttackLand()
    {
        golem.OnAttackLand();
        GameObject attack = Instantiate(attackPrefab,golem.SmashPosition.parent);
        attack.GetComponent<AttackController>().Init("Player",golem.Damage,0.2f,1);
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

    public void OnBeamRotationStart()=>golem.OnBeamRotationStart();

    public void OnBeamRotationEnd() => golem.CheckNextMove();

    public void OnIdleEnd() => golem.CheckNextMove();
}