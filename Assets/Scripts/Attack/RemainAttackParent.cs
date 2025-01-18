using UnityEngine;
using UnityEngine.PlayerLoop;

public class RemainAttackParent : AttackController
{
    protected GameObject child;
    protected float childDuration;
    protected float childAttackInterval;
    protected int childDamage;

    public void InitRemainParent(string TargetTag, int Damage, float destroyTime, int Penetration,GameObject child,float duration,float childAttackInterval,int childDamage)
    {
        this.child = child;
        childDuration = duration;
        this.childAttackInterval = childAttackInterval;
        this.childDamage = childDamage;
        base.Init(TargetTag,Damage,destroyTime,Penetration);
    }

    protected override void OnLastHit()
    {
        //子が出る位置を決める
        Vector3 childPosition = transform.position;
        childPosition.y = 0;

        GameObject remainAttackChild = Instantiate(child,childPosition,Quaternion.identity);
        remainAttackChild.GetComponent<RemainAttackChild>().InitAreaChild(targetTag,childDuration,damage,childAttackInterval);
        base.OnLastHit();
    }
}
