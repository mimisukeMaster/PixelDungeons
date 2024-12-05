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

        // 追跡時
        if (isChasing)
        {
            Vector3 distanceVec = chasingTarget.transform.position - transform.position;
            rb.linearVelocity = distanceVec.normalized * ChasingSpeed;

            // 一定時間ごとに弾を出す
            if(Time.time > nextShootTime && distanceVec.magnitude < AttackDistance)
            {
                GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity); 
                bullet.GetComponent<Rigidbody>().linearVelocity = distanceVec.normalized * ShootSpeed;
                bullet.GetComponent<AttackController>().Init("Player", Attack);

                Destroy(bullet, 5.0f);
                nextShootTime = Time.time + ShootInterval;
            }
        }
    }
    public override void OnDied()
    {
        GameObject item = Instantiate(DropItem, transform.position, Quaternion.identity); 
        item.GetComponent<MeshRenderer>().material.color = Color.yellow;
        base.OnDied();
    }
}
