using UnityEngine;

public class AttackController : MonoBehaviour
{
    private string targetTag;
    private int damage;

    // 初期化
    public void Init(string TargetTag, int Damage)
    {
        targetTag = TargetTag;
        damage = Damage;
    }

    /// <summary>
    /// 攻撃の処理
    /// </summary>
    /// <remarks>物理演算による衝突判定を用いる</remarks>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        // 敵自身の衝突（近距離）はNearAttackで行う
        if (gameObject.CompareTag("Enemy")) return;

        // タグで判定する
        if (other.gameObject.CompareTag(targetTag))
        {
            other.gameObject.GetComponent<HPController>().Damaged(damage);

            // UIを表示
            if(targetTag == "Enemy") DamageNumberManager.AddUI(damage, other.contacts[0].point);
            Destroy(gameObject);
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
                
                // UIを表示
                if (targetTag == "Enemy") DamageNumberManager.AddUI(damage, obj.transform.position);
                return;
            }
        }
    }
}
