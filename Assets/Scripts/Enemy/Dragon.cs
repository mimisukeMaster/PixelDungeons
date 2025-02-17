using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Dragon : EnemyController
{
    public enum DragonState
    {
        init,
        flyAround,
        hover,
        charge
    }
    [Header("Dragon")]
    public DragonState dragonState = DragonState.init;
    [Tooltip("通常時の高度")]
    public float normalAltitude;
    public float AltitudeRandomness;
    private Transform myTransform;
    public Animator animator;

    [Space(20)]
    [Header("追尾弾連射")]
    private Coroutine homingCoroutine;
    public GameObject homingPrefab;
    public int homingDamage;
    public float homingSpeed;
    public float homingAngle;
    public float homingFireRate;
    public int homingNumber;
    public float homingInterval;
    public float HomingSpread;

    [Header("本体狙い連射")]
    private Coroutine shotsCoroutine;
    public GameObject shotsPrefab;
    public int shotsDamage;
    public float shotsSpeed;
    public float shotsFireRate;
    public int shotsNumber;
    public float shotsSpread;
    public float shotsChargeTime;

    [Header("突進")]
    public bool isCharging = false;
    public int chargeDamage;

    [Header("Init")]
    public float ascentionSpeed;
    private float moveUpdateCountDown;//次の行動に向かう時間

    [Header("周回")]
    public float moveSpeed;
    public float moveRange;//移動目標のプレイヤーからの距離の上限
    public float rotationSpeed;
    private Vector3 MoveTargetPosition;//移動時の目標
    [Tooltip("移動の目標を更新する距離の二乗")]
    public float SqrMoveUpdateDistance;
    public int MinMoveUpdateTime;
    public int MaxMoveUpdateTime;


    [Header("ホバー")]
    public Transform WingRootTransform;
    public float HoverBodyRotationSpeed;
    public float HoverProbability;
    public float MinHoverTime;
    public float MaxHoverTime;


    [Header("突進")]//突進関連
    public float ChargeProbability;
    public float ChargeSpeed;
    public float ChargeRotation;
    private enum ChargeState
    {
        dive,
        charge
    }
    private ChargeState chargeState;


    [Header("首の向き")]
    public Transform NeckTransform;
    private Quaternion neckRotationTarget;
    public float NeckRotationSpeed;
    public float NeckFollowAngle = 50;

    protected override void Start()
    {
        base.Start();
        myTransform = transform;
        dragonState = DragonState.init;
    }

    protected override void Update()
    {
        DetectPlayer();
        if (Vector3.Angle(myTransform.forward, distanceVector) < NeckFollowAngle)
        {
            // 首をプレイヤーに向け、目の上側が正しく上を向くようにする　ビームは目の回転をもとにしているので再度回転を調整する
            neckRotationTarget = Quaternion.LookRotation(targetPlayer.transform.position - NeckTransform.position);
            neckRotationTarget = Quaternion.AngleAxis(90.0f, neckRotationTarget * Vector3.right) * neckRotationTarget;
        }
        // プレイヤーの方を向いていない場合は正面を向く
        else
        {
            neckRotationTarget = myTransform.rotation;
            neckRotationTarget = Quaternion.AngleAxis(90.0f, neckRotationTarget * Vector3.right) * neckRotationTarget;
        }

        NeckTransform.rotation = Quaternion.RotateTowards(NeckTransform.rotation,
                                            neckRotationTarget,
                                            NeckRotationSpeed * Time.deltaTime);

        switch(dragonState)
        {
            case DragonState.init:
                myTransform.position = myTransform.position + Vector3.up * ascentionSpeed * Time.deltaTime;
                if(myTransform.position.y >= normalAltitude)
                {
                    ChangeToFlyAround();
                }
                break;
            case DragonState.flyAround:
                FlyAround();
                break;
            case DragonState.hover:
                Hover();
                break;
            case DragonState.charge:
                Charge();
                break;
        }

    }

    private void FlyAround()
    {
        //移動する
        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;

        myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation,
                                                        Quaternion.LookRotation(MoveTargetPosition - myTransform.position),
                                                        rotationSpeed * Time.deltaTime);

        if((myTransform.position - MoveTargetPosition).sqrMagnitude <= SqrMoveUpdateDistance)
        {
            Vector3 moveTransform = targetPlayer.transform.position + new Vector3(Random.Range(-10,10),normalAltitude + Random.Range(-AltitudeRandomness,AltitudeRandomness),Random.Range(-10,10));
            MoveTargetPosition = moveTransform;
            moveUpdateCountDown--;
            Debug.Log(moveUpdateCountDown);
        }
        if(moveUpdateCountDown <= 0)
        {
            StopCoroutine(homingCoroutine);
            float randomNumber = Random.value;
            if(randomNumber > HoverProbability)
            {
                ChangeToHover();
            }
            else if(randomNumber > ChargeProbability)
            {
                ChangeToCharge();
            }
            else
            {
                ChangeToFlyAround();
            }
        }
    }

    private IEnumerator FlyAroundCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(homingInterval);
            for(int i = 0;i < homingNumber;i++)
            {
                Quaternion randomSpread = Quaternion.Euler(Random.Range(-HomingSpread,HomingSpread),Random.Range(-HomingSpread,HomingSpread),Random.Range(-HomingSpread,HomingSpread));
                GameObject homingBullet = Instantiate(homingPrefab,transform.position,randomSpread * Quaternion.Euler(-90,0,0) * myTransform.rotation);
                homingBullet.GetComponent<AttackController>().Init("Player",10,homingDamage,homingSpeed,homingAngle,1000);
                yield return new WaitForSeconds(homingFireRate);
            }
        }
    }

    private void ChangeToFlyAround()
    {
        animator.SetBool("Hover",false);
        Vector3 moveTransform = targetPlayer.transform.position + new Vector3(Random.Range(-10,10),normalAltitude + Random.Range(-AltitudeRandomness,AltitudeRandomness),Random.Range(-10,10));
        MoveTargetPosition = moveTransform;
        moveUpdateCountDown = Random.Range(MinMoveUpdateTime,MaxMoveUpdateTime);
        homingCoroutine = StartCoroutine(FlyAroundCoroutine());
        dragonState = DragonState.flyAround;
        Debug.Log(dragonState);
    }

    private void Hover()
    {
        myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation,
                                                        Quaternion.LookRotation(targetPlayer.transform.position - myTransform.position),
                                                        HoverBodyRotationSpeed * Time.deltaTime);

        // 羽をプレイヤーに向ける
        WingRootTransform.rotation = Quaternion.LookRotation(targetPlayer.transform.position - WingRootTransform.position);
                                            
        moveUpdateCountDown -= Time.deltaTime;

        if(moveUpdateCountDown <= 0)
        {
            StopCoroutine(shotsCoroutine);
            float randomNumber = Random.value;
            WingRootTransform.rotation = myTransform.rotation;
            if(randomNumber > ChargeProbability)
            {
                ChangeToCharge();
            }
            else
            {
                ChangeToFlyAround();
            }
        }
    }

    private IEnumerator HoverCoroutine()
    {
        yield return new WaitForSeconds(shotsChargeTime);
        for(int i = 0;i < shotsNumber;i++)
        {
            GameObject bullet = Instantiate(shotsPrefab,myTransform.position,Quaternion.identity);
            Quaternion randomDirection = Quaternion.Euler(Random.Range(-shotsSpread,shotsSpread),Random.Range(-shotsSpread,shotsSpread),Random.Range(-shotsSpread,shotsSpread));
            bullet.GetComponent<AttackController>().Init("Player",shotsDamage,100,1);
            bullet.GetComponent<Rigidbody>().linearVelocity = randomDirection * ((targetPlayer.transform.position-myTransform.position+ new Vector3(0,0.5f,0)).normalized*shotsSpeed);
            yield return new WaitForSeconds(shotsFireRate);
        }
    }

    private void ChangeToHover()
    {
        shotsCoroutine = StartCoroutine(HoverCoroutine());
        animator.SetBool("Hover",true);
        moveUpdateCountDown = Random.Range(MinHoverTime,MaxHoverTime);
        dragonState = DragonState.hover;
        Debug.Log(dragonState);
    }

    private void Charge()
    {
        //移動する
        myTransform.position += myTransform.forward * ChargeSpeed * Time.deltaTime;

        myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation,
                                                        Quaternion.LookRotation(MoveTargetPosition - myTransform.position),
                                                        ChargeRotation * Time.deltaTime);

        if((myTransform.position - MoveTargetPosition).sqrMagnitude <= SqrMoveUpdateDistance)
        {
            switch(chargeState)
            {
                case ChargeState.dive:
                    chargeState = ChargeState.charge;
                    Vector3 chargeTarget = targetPlayer.transform.position + targetPlayer.transform.position - myTransform.position;
                    chargeTarget.y = 1;
                    MoveTargetPosition = chargeTarget;
                    break;
                case ChargeState.charge:
                    isCharging = false;
                    ChangeToFlyAround();
                    break;
            }
        }
    }

    private void ChangeToCharge()
    {
        isCharging = true;
        animator.SetBool("Hover",false);
        chargeState = ChargeState.dive;
        Vector3 nextMoveTarget = myTransform.position;
        nextMoveTarget.y = 1;
        MoveTargetPosition = nextMoveTarget;
        dragonState = DragonState.charge;
        Debug.Log(dragonState);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(isCharging);
        if(isCharging && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HPController>().Damaged(chargeDamage,Vector3.zero);
            Debug.Log("Charge Damage");
        }
    }
}
