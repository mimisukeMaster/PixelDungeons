using UnityEngine;

public class DropAttackEnemy : EnemyController 
{
    [Space(20)]    
    public float Altitude = 10.0f;
    public float DropInterval = 1.0f;
    public GameObject Bullet; 
    public Animator animator;

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

        // 追跡
        if (isChasing)
        {
            // 飛行アニメーション開始
            animator.SetBool("Chasing", isChasing);

            // 方向ベクトル + 上昇成分 の移動
            Vector3 dirVector = (chasingTarget.transform.position - transform.position).normalized;
            rb.linearVelocity = (dirVector + Vector3.up) * ChasingSpeed;
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity.normalized);

            // 一定の高さになったら浮遊
            if (transform.position.y > Altitude) {
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                Debug.Log("a");
            }

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
    public override void OnDied()
    {
        GameObject item = Instantiate(DropItem, transform.position, Quaternion.identity); 
        base.OnDied();
    }
}
