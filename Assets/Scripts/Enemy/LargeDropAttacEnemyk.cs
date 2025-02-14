using UnityEngine;

public class LargeDropAttackEnemy : EnemyController
{
    [Space(20)]
    [Tooltip("飛行高度")]
    public float Altitude = 30.0f;
    [Tooltip("弾の落下時間間隔")]
    public float DropInterval = 1.0f;
    [Tooltip("弾のPrefab")]
    public GameObject Bullet;
    [Tooltip("アニメーション処理用")]
    public Animator animator;
    [Tooltip("飛び上がるときのアニメーション速度倍率")]
    public float FlyUpAnimationSpeed = 2.0f;

    [Header("爆発攻撃用パラメータ")]
    [Tooltip("爆発の半径")]
    public float BombRadius = 5.0f;
    [Tooltip("爆発ダメージ(追加ダメージ)")]
    public int BombDamage = 20;


    private float nextDropTime;

    protected override void Start()
    {
        base.Start();
        
        ChasingSpeed = 10.0f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    protected override void Update()
    {
        base.Update();

        // プレイヤー検知中は攻撃
        if (isChasing) FlyAndAttack();
    }

    private void FlyAndAttack()
    {
        // アニメーション切替
        animator.SetBool("Chasing", true);

        // XZ 平面でプレイヤー方向 + 上方向へ移動
        distanceVector.y = 0f;
        distanceVector.Normalize();
        Vector3 flyVelocity = (distanceVector + Vector3.up) * ChasingSpeed;

        // 高度以上になったら上昇しない
        if (transform.position.y >= Altitude)
        {
            animator.SetFloat("WingSpeed", 1.0f);
            flyVelocity.y = 0f;
        }
        else
        {
            animator.SetFloat("WingSpeed", FlyUpAnimationSpeed);
        }

        rb.linearVelocity = flyVelocity;

        // 滑らかに進行方向を向く
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(rb.linearVelocity.normalized),
                Time.deltaTime * 10.0f
            );
        }

        // 一定間隔で弾(爆発弾)を落とす
        if (Time.time > nextDropTime)
        {
            DropBomb();
            nextDropTime = Time.time + DropInterval;
        }
    }

    /// <summary>
    /// 爆発する弾を生成し、AttackController の爆発用 Init を呼び出す
    /// </summary>
    private void DropBomb()
    {
    
        GameObject bulletObj = Instantiate(Bullet, transform.position, Quaternion.identity);

    
        bulletObj.GetComponent<AttackController>().Init(
            "Player",       // 攻撃対象のタグ
            5.0f,           // 弾が何秒後に消えるか
            Damage,         // 直撃ダメージ
            BombRadius,     // 爆発半径
            BombDamage      // 爆発追加ダメージ
        );
    }
}
