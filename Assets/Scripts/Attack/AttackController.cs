using UnityEngine;

public class AttackController : MonoBehaviour
{
    public AudioClip BallFireImpactSE;
    protected string targetTag = "";
    protected int damage;

    [Tooltip("攻撃が敵を貫通する数")]
    private int penetration = 100;
    private AudioSource audioSource;

    // 初期化
    public virtual void Init(string TargetTag, int Damage, float destroyTime,int Penetration)
    {
        targetTag = TargetTag;
        damage = Damage;
        penetration = Penetration;
        Destroy(gameObject, destroyTime);
    }

    private void Start()
    {
        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
    }

    /// <summary>
    /// 攻撃の処理
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter(Collider other)
    {
        // 自分自身との衝突と自分が敵の場合は無視
        if (other.gameObject.CompareTag(gameObject.tag) ||gameObject.CompareTag("Enemy")) return;

        // タグで判定する
        if (other.CompareTag(targetTag))
        {
            other.GetComponentInParent<HPController>().Damaged(damage);
            // UIを表示
            if(targetTag == "Enemy") DamageNumberManager.AddUI(damage, transform.position);
        }
        penetration--;
        Debug.Log(penetration);
        if(penetration <= 0)
        {
            OnLastHit();
        }
    }

    protected virtual void OnLastHit() 
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 攻撃の処理
    /// </summary>
    /// <remarks>物理演算による衝突判定が使えない場合</remarks>
    /// <param name="DetectPos"></param>
    /// <param name="DetectRange"></param>
    public void NearAttack(Vector3 DetectPos, float DetectRange)
    {
        Collider[] hitObjs = Physics.OverlapSphere(DetectPos, DetectRange);
        foreach (var obj in hitObjs)
        {
            if (obj.CompareTag(targetTag))
            {                
                obj.GetComponent<HPController>().Damaged(damage);
                
                // UIを表示
                if (targetTag == "Enemy") DamageNumberManager.AddUI(damage, obj.transform.position);
                return;
            }
        }
    }
}
