using UnityEngine;

/// <summary>
/// 遠距離攻撃の敵の処理
/// </summary>
/// <remarks>遠距離攻撃は遠方から弾を出す</remarks>
public class FarAttackEnemy : EnemyController
{
    [Space(20)]
    public float AttackDistance = 3.0f;
    public float ShootSpeed = 10.0f;
    public float ShootInterval = 1.0f;
    public GameObject Bullet; 

    private float nextShootTime;
    

    protected override void Update() 
    {
        // 敵のUpdate()を実行
        base.Update();

        // 追跡処理
        if (isChasing)
        {
            Vector3 dirVector = (chasingTarget.transform.position - transform.position).normalized;
            rb.linearVelocity = dirVector * ChasingSpeed;
        }

        // 弾を出す処理
        if(Time.time > nextShootTime && isChasing)
        { 
            Vector3 distanceVec = chasingTarget.transform.position - transform.position;
            if(distanceVec.magnitude < AttackDistance)
            {
                GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity); 
                bullet.GetComponent<Rigidbody>().linearVelocity = distanceVec.normalized * ShootSpeed;

                Destroy(bullet, 5.0f);
            }
            nextShootTime = Time.time + ShootInterval;
        }
    }
}
