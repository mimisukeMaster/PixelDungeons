using UnityEngine;

public class DropAttackEnemy : EnemyController 
{
    [Space(20)]    
    public float Altitude = 10.0f;
    public float DropInterval = 1.0f;
    public GameObject Bullet; 

    private float nextDropTime;

    
    protected override void Start()
    {   
        // 継承元のStart()を実行
        base.Start();

        transform.position = new Vector3(transform.position.x, Altitude, transform.position.z);
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        // 検知範囲を上書きする、高度の約2/√3倍
        DetectionRadius = Altitude * 1.15f;
    }

    protected override void Update() 
    {
        // 継承元のUpdate()を実行
        base.Update();

        // 追跡
        if (isChasing)
        {
            Vector3 dirVector = (chasingTarget.transform.position - transform.position).normalized;
            rb.linearVelocity = dirVector * ChasingSpeed;

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
