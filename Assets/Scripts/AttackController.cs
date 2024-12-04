using UnityEngine;

public class AttackController : MonoBehaviour
{
    private string targetTag;
    private int damage;
    private float speed;
    private Transform myTransform;

    // 初期化
    public void Init(string TargetTag, int Damage, float Speed)
    {
        targetTag = TargetTag;
        damage = Damage;
        speed = Speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        //タグで判定する
        if (other.gameObject.CompareTag(targetTag))
        {
            other.gameObject.GetComponent<HPController>().Damaged(damage);
            Destroy(gameObject);
        }
    }
}
