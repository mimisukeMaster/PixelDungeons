using UnityEngine;
using UnityEngine.PlayerLoop;

public class AttackController : MonoBehaviour
{
    private int TargetLayer;
    private float damage;
    private float speed;
    private Transform myTransform;

    //初期化
    public void Init(int TargetLayer, float damage, float speed)
    {
        this.TargetLayer = TargetLayer;
        this.damage = damage;
        this.speed = speed;
    }

    private void Update()
    {
        if (myTransform == null)
        {
            myTransform = transform;
        }

        myTransform.position += Vector3.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //レイヤーで判定する
        if (other.gameObject.layer == TargetLayer)
        {
            other.GetComponent<HPController>().Damaged((int)damage);
            Destroy(gameObject);
        }
    }
}
