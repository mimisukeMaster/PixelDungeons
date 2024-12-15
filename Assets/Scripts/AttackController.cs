using UnityEngine;

public class AttackController : MonoBehaviour
{
    private string targetTag;
    private int damage;
    private int penetration;

    // 初期化
    public void Init(string TargetTag, int Damage,int Penetration,float DestroyTime)
    {
        targetTag = TargetTag;
        damage = Damage;
        penetration = Penetration;
        Debug.Log(gameObject.name+":"+DestroyTime.ToString());
        Destroy(gameObject,DestroyTime);
    }

    /// <summary>
    /// 攻撃の処理
    /// </summary>
    /// <remarks>物理演算による衝突判定を用いる</remarks>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) 
    {
        // 敵自身の衝突（近距離）はNearAttackで行う
        if (gameObject.CompareTag("Enemy")) return;

        // タグで判定する
        if (other.gameObject.CompareTag(targetTag))
        {
            Debug.Log("Attack");
            other.gameObject.GetComponent<HPController>().Damaged(damage);

            penetration--;
            if(penetration <=0)
            {
                Debug.Log("Destroy:"+gameObject);
                Destroy(gameObject);
            }
        }
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
                return;
            }
        }
    }
}
