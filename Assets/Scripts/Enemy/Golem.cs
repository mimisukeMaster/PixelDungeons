using Unity.VisualScripting;
using UnityEngine;
public class Golem : MiddleBossEnemy
{
    [Tooltip("目のボーン")]
    public Transform Eye;

    public float attackrange = 2;

    private bool hasMoved = false;
    [Tooltip("攻撃時間間隔")]
    public float SmashInterval = 1.0f;
    [Tooltip("攻撃の中心")]
    public Transform SmashPosition;
    [Tooltip("攻撃半径")]
    public float smashRange;
    private float nextSmashTime;

    public enum GolemState
    {
        idle,
        move,
        turn,
        attack
    }
    public GolemState golemState;

    protected override void Update()
    {
        DetectPlayer();
        if(isChasing)
        {
            //目をプレイヤーに向ける
            if(Vector3.Angle(transform.forward,targetPlayer.transform.position - transform.position)<90)
            {
                Eye.transform.rotation = Quaternion.LookRotation(targetPlayer.transform.position - Eye.transform.position);
                Eye.transform.rotation = Quaternion.AngleAxis(90,Eye.transform.right) * Eye.transform.rotation;
            }
            else Eye.transform.rotation = Quaternion.Euler(90,0,0);

            if(!hasMoved)
            {
                CheckNextMove();
                hasMoved = true;
            }
        }
    }

    private void CheckNextMove()
    {
        if((transform.position - targetPlayer.transform.position).magnitude < attackrange) StartAttack();
        else StartMove();
    }

    private void StartAttack()
    {
        //攻撃処理
        Vector3 destVec = distanceVector;
        destVec.y = 0;
        gameObject.transform.forward = destVec;
        animator.SetTrigger("Attack");
        golemState = GolemState.attack;
    }

    private void StartMove()
    {
        //移動処理
        Vector3 destVec = targetPlayer.transform.position - transform.position;
        destVec.y = 0;
        gameObject.transform.forward = destVec;
        animator.SetTrigger("Move");
        golemState = GolemState.move;
    }

    public void OnAttackLand()
    {
        attackController.NearAttack(SmashPosition.position,smashRange);
    }

    public void OnAttackEnd()
    {
        CheckNextMove();
    }

    public void OnMoveEnd()
    {
        transform.position += transform.forward * 5;//前に進む 5はアニメーションの腕のスパン
        CheckNextMove();
    }
}