using UnityEngine;

public class NearAttackEnemy : EnemyController
{
    [Space(20)]
    [Tooltip("攻撃開始距離")]
    public float AttackDistance = 1.0f;
    [Tooltip("攻撃時間間隔")]
    public float BlowInterval = 1.5f;

    private Animator animator;
    private AttackController attackController;
    private float nextBlowTime;

    protected override void Start()
    {
        base.Start();
        animator = gameObject.GetComponentInChildren<Animator>();
        attackController = gameObject.GetComponent<AttackController>();
        attackController.Init("Player", Attack);
    }

    protected override void Update()
    {
        // ランダム移動はせず検知のみ
        DetectPlayer();

        if (isChasing)
        {
            
            // 追跡
            animator.SetBool("Move", true);

            Vector3 destVec = distanceVector;
            destVec.y = 0;
            destVec.Normalize();
            rb.linearVelocity = destVec * ChasingSpeed;

            // 攻撃
            if (Time.time > nextBlowTime && distanceVector.magnitude < AttackDistance)
            {
                gameObject.transform.forward = destVec;
                animator.SetTrigger("Attack");

                attackController.NearAttack(transform.position, AttackDistance);
                nextBlowTime = Time.time + BlowInterval;
            }
        }
        else animator.SetBool("Move", false);
    }
}
