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
            //頭をプレイヤーに向ける
            Eye.transform.rotation = Quaternion.FromToRotation(Vector3.up,targetPlayer.transform.position - Eye.position);
            //Eye.rotation = Quaternion.LookRotation(targetPlayer.transform.position - Eye.position);

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

    public void OnAttack()
    {
        Debug.Log("AttackLand");
        attackController.NearAttack(SmashPosition.position,smashRange);
        CheckNextMove();
    }

    public void OnMoveEnd()
    {
        Debug.Log("Move End");
        transform.position += transform.forward * 5;//前に進む 5はアニメーションの腕のスパン
        CheckNextMove();
    }
}