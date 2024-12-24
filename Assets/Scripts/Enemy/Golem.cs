using UnityEngine;
public class Golem : MiddleBossEnemy
{
    [Tooltip("目のボーン")]
    public Transform Eye;
    [Tooltip("攻撃モーションを始める距離")]
    public float AttackMotionRange = 8.0f;
    [Tooltip("攻撃時間間隔")]
    public float SmashInterval = 1.0f;
    [Tooltip("攻撃の中心")]
    public Transform SmashPosition;
    [Tooltip("攻撃半径")]
    public float SmashRange = 4.0f;

    private bool isInitChasing = true;

    protected override void Update()
    {
        DetectPlayer();
        if (isChasing)
        {
            //目をプレイヤーに向ける
            Eye.transform.forward = distanceVector;
            
            if (isInitChasing)
            {
                CheckNextMove();
                isInitChasing = false;
            }

            // アニメーションの関係でプレイヤーに近づきすぎた時は即座に攻撃の挙動
            if (distanceVector.magnitude < AttackMotionRange)
            {
                Debug.Log("tikai");
                animator.StopPlayback();
                animator.SetTrigger("Attack");
            }
        }
    }

    /// <summary>
    /// プレイヤーに十分近づいたかを調べ、移動または攻撃の挙動をする
    /// </summary>
    public void CheckNextMove()
    {
        if (!isChasing)
        {
            animator.SetTrigger("Idle");
            return;
        }

        Vector3 destVec = distanceVector;
        destVec.y = 0;
        gameObject.transform.forward = destVec;

        if (distanceVector.magnitude < AttackMotionRange)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("Move");
        }
    }
}