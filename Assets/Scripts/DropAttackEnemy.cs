using UnityEngine;

public class DropAttackEnemy : EnemyController 
{
    [Space(20)]
    [Tooltip("飛行高度")]
    public float Altitude = 10.0f;
    [Tooltip("弾の落下時間間隔")]
    public float DropInterval = 1.0f;
    [Tooltip("弾のPrefab")]
    public GameObject Bullet; 
    [Tooltip("アニメーション処理用")]
    public Animator animator;
    [Tooltip("飛び上がるときのアニメーション倍率")]
    public float FlyUpAnimationSpeed;

    private float nextDropTime;

    
    protected override void Start()
    {   
        // 継承元のStart()を実行
        base.Start();

        // 回転固定
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    protected override void Update() 
    {
        // 継承元のUpdate()を実行
        base.Update();

        // 攻撃
        if (isChasing) FlyAndAttack();
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void FlyAndAttack()
    {
        // アニメーション切替
        animator.SetBool("Chasing", true);

        // 方向ベクトル + 上昇成分 の移動
        distanceVector.y = 0f;
        if (distanceVector.magnitude > 0.9f)
        {
            distanceVector.Normalize();
            rb.linearVelocity = (distanceVector + Vector3.up) * ChasingSpeed;

            // 滑らかに進行方向に向きを合わせる
            transform.rotation = Quaternion.Slerp(
            transform.rotation, Quaternion.LookRotation(rb.linearVelocity.normalized), Time.deltaTime * 10.0f);
        }
        else rb.linearVelocity = Vector3.zero;

        // 高度制限
        if (transform.position.y > Altitude)
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
            animator.SetFloat("WingSpeed", 1);
        }
        else animator.SetFloat("WingSpeed", FlyUpAnimationSpeed);

        // 一定時間ごとに弾を出す
        if(Time.time > nextDropTime)
        {
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<AttackController>().Init("Player", Attack);

            Destroy(bullet, 5.0f);

            nextDropTime = Time.time + DropInterval;
        }
    }
}
