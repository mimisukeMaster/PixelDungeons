using Unity.VisualScripting;
using UnityEngine;

public class BombAttack : AttackController
{
    private float radius;

    public void InitBomb(string TargetTag, int Damage, float destroyTime,int Penetration,float explosionRadius)
    {
        base.Init(TargetTag, Damage, destroyTime,Penetration);
        radius = explosionRadius;
    }

    protected override void OnLastHit()
    {
        Collider[] hitObjs = Physics.OverlapSphere(transform.position, radius);
        foreach (var obj in hitObjs)
        {
            if (obj.CompareTag(targetTag))
            {
                obj.GetComponentInParent<HPController>().Damaged(damage);
                
                // UIを表示
                if (targetTag == "Enemy") DamageNumberManager.AddUI(damage, obj.transform.position);
            }
        }
        base.OnLastHit();
    }
}
