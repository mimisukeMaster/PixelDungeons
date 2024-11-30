using UnityEngine;

public class DoropAttackEnemy : EnemyController 
{
    
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

        if(Time.time > nextDropTime && isChasing)
        {
            // 生成と同時に落下していく
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);

            Destroy(bullet, 5.0f);

            nextDropTime = Time.time + DropInterval;
        }
    }
}
