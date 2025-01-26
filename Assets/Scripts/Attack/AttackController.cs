using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour
{
    public AudioClip ImpactSE;
    protected string targetTag = "";
    protected Transform myTransform;
    protected Rigidbody myRb;

    //ステータス
    protected int damage;
    protected float speed;
    private int penetration = 100;

    protected bool isHoming = false;
    protected float homingAngle;
    protected float detectionDistance;
    protected Vector3 targetPosition;
    protected float DetectionFrequency = 0.2f;
    protected float nextDetectTime = 0;

    protected bool isBeam = false;
    protected LineRenderer beamLineRenderer;
    protected float beamChargeTime;
    protected float beamEmissionTime;
    protected float beamLength;

    protected bool isBomb = false;
    protected float bombRadius;
    protected int bombDamage;

    protected bool isRemain = false;
    protected GameObject areaChild;
    protected float areaChildDuration;
    protected float areaChildAttackInterval;
    protected int areaChildDamage;
    // \ステータス

    private AudioSource audioSource;

    // シンプルな初期化
    public virtual void Init(string targetTag, int damage, float destroyTime,int penetration)
    {
        this.targetTag = targetTag;
        this.damage = damage;
        this.penetration = penetration;
        Destroy(gameObject, destroyTime);

        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
        myTransform = transform;
        myRb = GetComponent<Rigidbody>();

        if(isBeam)
        {
            StartCoroutine(BeamAttack());
            beamLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            beamLineRenderer.enabled = false;
        }
    }

    //細かい初期化
    public virtual void Init(string targetTag,float destroyTime,int damage,float speed,bool isHoming,float homingAngle,float detectionDistance,bool isBeam,LineRenderer beamLineRenderer,float beamChargeTime,float beamEmissionTime,float beamLength,bool isBomb,float bombRadius,int bombDamage,bool isRemain,GameObject areaChild,float areaDuration,float areaChildAttackInterval,int areaChildDamage)
    {
        this.targetTag = targetTag;
        this.damage = damage;
        this.speed = speed;
        this.isHoming = isHoming;
        this.homingAngle = homingAngle;
        this.detectionDistance = detectionDistance;
        this.isBeam = isBeam;
        this.beamLineRenderer = beamLineRenderer;
        this.beamChargeTime = beamChargeTime;
        this.beamEmissionTime = beamEmissionTime;
        this.beamLength = beamLength;
        this.isBomb = isBomb;
        this.bombRadius = bombRadius;
        this.bombDamage = bombDamage;
        this.isRemain = isRemain;
        this.areaChild = areaChild;
        this.areaChildDuration = areaDuration;
        this.areaChildAttackInterval = areaChildAttackInterval;
        this.areaChildDamage = areaChildDamage;
        Destroy(gameObject,destroyTime);

        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
        myTransform = transform;
        myRb = GetComponent<Rigidbody>();

        if(isBeam)
        {
            beamLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            beamLineRenderer.enabled = false;

            StartCoroutine(BeamAttack());
        }
    }

    //追尾のみの攻撃の初期化
    public virtual void Init(string targetTag,float destroyTime,int damage,float speed,float homingAngle,float detectionDistance)
    {
        isHoming = true;
        this.targetTag = targetTag;
        this.damage = damage;
        this.speed = speed;
        this.homingAngle = homingAngle;
        this.detectionDistance = detectionDistance;
        Destroy(gameObject,destroyTime);

        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
        myTransform = transform;
        myRb = GetComponent<Rigidbody>();

    }

    //ビームのみの攻撃の初期化
    public virtual void Init(string targetTag,float destroyTime,int damage,LineRenderer beamLineRenderer,float beamChargeTime,float beamEmissionTime,float beamLength)
    {
        isBeam = true;
        this.targetTag = targetTag;
        this.damage = damage;
        this.beamLineRenderer = beamLineRenderer;
        this.beamChargeTime = beamChargeTime;
        this.beamEmissionTime = beamEmissionTime;
        this.beamLength = beamLength;
        Destroy(gameObject,destroyTime);

        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
        myTransform = transform;
        myRb = GetComponent<Rigidbody>();

        beamLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        beamLineRenderer.enabled = false;

        StartCoroutine(BeamAttack());
    }

    //爆発のみの攻撃の初期化
    public virtual void Init(string targetTag,float destroyTime,int damage,float bombRadius,int bombDamage)
    {
        isBomb = true;
        this.targetTag = targetTag;
        this.damage = damage;
        this.bombRadius = bombRadius;
        this.bombDamage = bombDamage;
        Destroy(gameObject,destroyTime);

        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
        myTransform = transform;
        myRb = GetComponent<Rigidbody>();
    }

    //残留のみの攻撃の初期化
    public virtual void Init(string targetTag,float destroyTime,int damage,GameObject areaChild,float areaDuration,float areaChildAttackInterval,int areaChildDamage)
    {
        isRemain = true;
        this.targetTag = targetTag;
        this.damage = damage;
        this.areaChild = areaChild;
        areaChildDuration = areaDuration;
        this.areaChildAttackInterval = areaChildAttackInterval;
        this.areaChildDamage = areaChildDamage;
        Destroy(gameObject,destroyTime);

        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
        myTransform = transform;
        myRb = GetComponent<Rigidbody>();
    }

    //Item_Weaponを利用した初期化
    public virtual void Init(string targetTag,float destroyTime, Item_Weapon weapon)
    {
        this.targetTag = targetTag;
        damage = weapon.Damage;
        speed = weapon.Speed;
        penetration = weapon.Penetration;
        isHoming = weapon.IsHoming;
        homingAngle = weapon.HomingAngle;
        detectionDistance = weapon.DetectionDistance;
        isBeam = weapon.isBeam;
        beamLineRenderer = weapon.beamLineRenderer;
        beamChargeTime = weapon.beamChargeTime;
        beamEmissionTime = weapon.beamEmissionTime;
        beamLength = weapon.beamLength;
        isBomb = weapon.IsBomb;
        bombRadius = weapon.BombRadius;
        bombDamage = weapon.BombDamage;
        isRemain = weapon.IsRemain;
        areaChild = weapon.AreaChild;
        areaChildDuration = weapon.AreaDuration;
        areaChildAttackInterval = weapon.AreaChildAttackInterval;
        areaChildDamage = weapon.AreaChildDamage;
        Destroy(gameObject,destroyTime);

        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
        myTransform = transform;
        myRb = GetComponent<Rigidbody>();

        if(isBeam)
        {
            beamLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            beamLineRenderer.enabled = false;

            StartCoroutine(BeamAttack());
        }
    }

    private void Update() 
    {
        if(isHoming)Homing();
    }

    /// <summary>
    /// 攻撃の処理
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter(Collider other)
    {
        // 自分自身との衝突と自分が敵の場合は無視
        if (other.gameObject.CompareTag(gameObject.tag) ||gameObject.CompareTag("Enemy")) return;
        //地形に当たったら削除
        if(other.gameObject.CompareTag("StageObject"))
        {
            OnLastHit();
            return;
        }

        // タグで判定する
        if (other.CompareTag(targetTag))
        {
            other.GetComponentInParent<HPController>().Damaged(damage);
            // UIを表示
            if(targetTag == "Enemy") DamageNumberManager.AddUI(damage, transform.position);
        }
        penetration--;
        if(penetration <= 0)
        {
            OnLastHit();
        }
    }

    //
    protected virtual void OnLastHit() 
    {
        if(isBomb)
        {
            Collider[] hitObjs = Physics.OverlapSphere(myTransform.position, bombRadius);
            foreach (var obj in hitObjs)
            {
                if (obj.CompareTag(targetTag))
                {
                    obj.GetComponentInParent<HPController>().Damaged(damage);
                    
                    // UIを表示
                    if (targetTag == "Enemy") DamageNumberManager.AddUI(damage, obj.transform.position);
                }
            }
        }

        if(isRemain)
        {
            //子が出る位置を決める
            Vector3 childPosition = transform.position;
            childPosition.y = 0;

            GameObject remainAttackChild = Instantiate(areaChild,childPosition,Quaternion.identity);
            remainAttackChild.GetComponent<RemainAttackChild>().InitAreaChild(targetTag,areaChildDuration,areaChildDamage,areaChildAttackInterval);
        }
        if(!isBeam)Destroy(gameObject);
    }

    protected virtual void Homing()
    {
        if (Time.time > nextDetectTime)
        {
            if (targetTag == "Enemy") targetPosition = DetectEnemy();
            else if (targetTag == "Player") targetPosition = PlayerController.PlayerObject.transform.position;

            nextDetectTime += DetectionFrequency;
        }
        if (targetPosition != Vector3.zero)
        {
            myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation,
                Quaternion.LookRotation(targetPosition - myTransform.position),
                homingAngle * Time.deltaTime);
        }

        myRb.linearVelocity = myTransform.forward * speed;
    }

    private Vector3 DetectEnemy()
    {
        //一番近い敵を識別する
        Vector3 closestEnemyPosition = Vector3.zero;
        float sqrClosestDistance = detectionDistance;//弾から一番近い敵までの距離
        foreach (GameObject enemy in SpawnManager.EnemiesInStage)
        {
            //敵までの距離
            float distance = (myTransform.position - enemy.transform.position).sqrMagnitude;
            if (distance < sqrClosestDistance)
            {
                closestEnemyPosition = enemy.transform.position;
                sqrClosestDistance = distance;
            }
        }
        return closestEnemyPosition;
    }

    private IEnumerator BeamAttack()
    {
        beamLineRenderer.enabled = true;

        // 予測線の準備
        SetBeamAppearance(0.5f, 0.5f, Color.cyan, Color.blue);
        Vector3 beamRootPos = transform.parent.position;
        Vector3 beamTipPos = transform.parent.position + transform.parent.forward * -beamLength;
        beamLineRenderer.SetPosition(0, beamRootPos);
        beamLineRenderer.SetPosition(1, beamTipPos);

        yield return new WaitForSeconds(beamChargeTime);

        // 実際に攻撃 ビームのパラメータはInitBeamで設定済み
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, beamLength * 2.0f);

        yield return new WaitForSeconds(beamEmissionTime);

        // パーティクルが消えないバグ（条件不明）対処
        transform.parent.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);

        Destroy(transform.parent.gameObject);
    }

    /// <summary>
    /// ビームの外観を設定
    /// </summary>
    private void SetBeamAppearance(float startWidth, float endWidth, Color startColor, Color endColor)
    {
        beamLineRenderer.startWidth = startWidth;
        beamLineRenderer.endWidth = endWidth;
        beamLineRenderer.startColor = startColor;
        beamLineRenderer.endColor = endColor;
    }
}
