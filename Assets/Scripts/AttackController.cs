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

    private void OnCollisionEnter(Collision other)
    {
        // タグで判定する
        if (other.gameObject.CompareTag(targetTag))
        {
            other.gameObject.GetComponent<HPController>().Damaged(damage);
            Destroy(gameObject);
        }
    }
}
