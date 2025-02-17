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
    public DragonState dragonState = DragonState.init;
    [Tooltip("通常時の高度")]
    public float normalAltitude;
    public float AltitudeRandomness;
    private Transform myTransform;
    public Transform MoveTargetSphere;//デバッグ用
    public Animator animator;
    [Header("Init")]
    public float ascentionSpeed;
    private float moveUpdateCountDown;//次の行動に向かう時間

    [Header("move")]
    public float moveSpeed;
    public float moveRange;//移動目標のプレイヤーからの距離の上限
    public float rotationSpeed;
    private Vector3 MoveTargetPosition;//移動時の目標
    [Tooltip("移動の目標を更新する距離の二乗")]
    public float SqrMoveUpdateDistance;
    public int MinMoveUpdateTime;
    public int MaxMoveUpdateTime;

    [Header("hover")]
    public Transform WingRootTransform;
    public float HoverBodyRotationSpeed;
    public float HoverProbability;
    public float MinHoverTime;
    public float MaxHoverTime;

    [Header("Charge")]
    public float ChargeProbability;
    public float ChargeSpeed;
    public float ChargeRotation;
    private enum ChargeState
    {
        dive,
        charge
    }
    private ChargeState chargeState;

    [Header("首")]
    public Transform NeckTransform;
    private Quaternion neckRotationTarget;
    public float NeckRotationSpeed;
    public float NeckFollowAngle = 50;

    protected override void Start()
    {
        base.Start();
        MoveTargetSphere = GameObject.Find("DragonMoveTransformSphere").transform;
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
                    moveUpdateCountDown = Random.Range(MinMoveUpdateTime,MaxMoveUpdateTime);
                    MoveTargetPosition = targetPlayer.transform.position + new Vector3(Random.Range(-10,10),normalAltitude + Random.Range(-AltitudeRandomness,AltitudeRandomness),Random.Range(-10,10));
                    dragonState = DragonState.flyAround;
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

        MoveTargetSphere.position = MoveTargetPosition;
        if((myTransform.position - MoveTargetPosition).sqrMagnitude <= SqrMoveUpdateDistance)
        {
            Vector3 moveTransform = targetPlayer.transform.position + new Vector3(Random.Range(-10,10),normalAltitude + Random.Range(-AltitudeRandomness,AltitudeRandomness),Random.Range(-10,10));
            MoveTargetPosition = moveTransform;
            moveUpdateCountDown--;
            Debug.Log(moveUpdateCountDown);
        }
        if(moveUpdateCountDown <= 0)
        {
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

    private void ChangeToFlyAround()
    {
        animator.SetBool("Hover",false);
        Vector3 moveTransform = targetPlayer.transform.position + new Vector3(Random.Range(-10,10),normalAltitude + Random.Range(-AltitudeRandomness,AltitudeRandomness),Random.Range(-10,10));
        MoveTargetPosition = moveTransform;
        moveUpdateCountDown = Random.Range(MinMoveUpdateTime,MaxMoveUpdateTime);
        dragonState = DragonState.flyAround;
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

    private void ChangeToHover()
    {
        Debug.Log(dragonState);
        animator.SetBool("Hover",true);
        moveUpdateCountDown = Random.Range(MinHoverTime,MaxHoverTime);
        dragonState = DragonState.hover;
    }

    private void Charge()
    {
        //移動する
        myTransform.position += myTransform.forward * ChargeSpeed * Time.deltaTime;

        myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation,
                                                        Quaternion.LookRotation(MoveTargetPosition - myTransform.position),
                                                        ChargeRotation * Time.deltaTime);
        
        MoveTargetSphere.position = MoveTargetPosition;

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
                    ChangeToFlyAround();
                    break;
            }
        }
    }

    private void ChangeToCharge()
    {
        Debug.Log(dragonState);
        animator.SetBool("Hover",false);
        chargeState = ChargeState.dive;
        Vector3 nextMoveTarget = myTransform.position;
        nextMoveTarget.y = 1;
        MoveTargetPosition = nextMoveTarget;
        dragonState = DragonState.charge;
    }
}
