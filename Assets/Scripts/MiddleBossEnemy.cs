using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class MiddleBossEnemy : EnemyController
{
    [Tooltip("攻撃時間間隔")]
    public float SmashInterval = 1.0f;
    [Tooltip("攻撃の中心")]
    public Transform SmashArea;
    [Tooltip("攻撃半径")]
    public float smashRange;

    private Animator animator;
    private AttackController attackController;
    private float nextSmashTime;

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        attackController.Init("Player", Attack);

    }
    protected override void Update()
    {
        // 一定時間ごとにプレイヤー検知
        DetectPlayer();

        if (isChasing)
        {
            // 追跡
            animator.SetBool("Move", true);

            Vector3 destVec = distanceVector;
            destVec.y = 0;
            destVec.Normalize();
            rb.linearVelocity = destVec * ChasingSpeed;
            transform.forward = destVec;


            // 攻撃 中身はMiddleBossAttackEventHandler.csから呼び出す
            if(Time.time > nextSmashTime)
            {
                animator.SetTrigger("Attack");
                nextSmashTime = Time.time + SmashInterval;
            }
        }
        else animator.SetBool("Move", false);
    }
}
