using UnityEngine;
public class Golem : MiddleBossEnemy
{
    [Tooltip("目のボーン")]
    public Transform Eye;
    [Tooltip("攻撃モーションを始める距離")]
    public float AttackMotionRange = 5.0f;
    [Tooltip("攻撃時間間隔")]
    public float SmashInterval = 1.0f;
    [Tooltip("攻撃の中心")]
    public Transform SmashPosition;
    [Tooltip("攻撃半径")]
    public float SmashRange = 4.0f;

    private enum GolemState
    {
        idle,
        move,
        turn,
        attack
    }
    private GolemState golemState;
    private bool isInitChasing = true;

    protected override void Update()
    {
        DetectPlayer();
        if (isChasing)
        {
            //目をプレイヤーに向ける
            if (Vector3.Angle(transform.forward, distanceVector) < 90)
            {
                Eye.transform.forward = distanceVector;
            }
            else Eye.transform.rotation = Quaternion.Euler(90,0,0);

            if (isInitChasing)
            {
                CheckNextMove();
                isInitChasing = false;
            }
        }
    }

    /// <summary>
    /// プレイヤーに十分近づいたかを調べ、移動または攻撃の挙動をする
    /// </summary>
    public void CheckNextMove()
    {
        Vector3 destVec = distanceVector;
        destVec.y = 0;
        gameObject.transform.forward = destVec;

        if (distanceVector.magnitude < AttackMotionRange)
        {
            animator.SetTrigger("Attack");
            golemState = GolemState.attack;
        }
        else
        {
            animator.SetTrigger("Move");
            golemState = GolemState.move;
        }
    }
}