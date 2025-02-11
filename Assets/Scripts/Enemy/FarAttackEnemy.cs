using UnityEngine;

public class FarAttackEnemy : EnemyController
{
    [Space(20)]
    [Tooltip("攻撃を開始する距離")]
    public float AttackDistance = 3.0f;
    [Tooltip("弾の速さ")]
    public float ShootSpeed = 10.0f;
    [Tooltip("弾の発射時間間隔")]
    public float ShootInterval = 1.0f;
    [Tooltip("弾のPrefab")]
    public GameObject Bullet;

    private float nextShootTime;
    

    protected override void Update() 
    {
        // 敵のUpdate()を実行
        base.Update();

        // 攻撃
        if (isChasing) ChaseAndAttack();
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void ChaseAndAttack()
    {
        // 追跡
        rb.linearVelocity = distanceVector.normalized * ChasingSpeed;

        // 一定時間ごとに弾を出す
        if(Time.time > nextShootTime && distanceVector.magnitude < AttackDistance)
        {
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity); 
            bullet.GetComponent<Rigidbody>().linearVelocity = distanceVector.normalized * ShootSpeed;
            bullet.GetComponent<AttackController>().Init("Player", Damage, 5.0f,1);

            Destroy(bullet, 5.0f);
            nextShootTime = Time.time + ShootInterval;
        }
    }

    public override void OnDied(GameObject gameObject)
    {
        if (Random.Range(0f, 1.0f) < DropProbability) Instantiate(DropItem, transform.position, Quaternion.identity);
        base.OnDied(gameObject);
    }
}
