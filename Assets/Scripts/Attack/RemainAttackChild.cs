using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RemainAttackChild : MonoBehaviour
{
    private Dictionary<HPController,float> targets = new Dictionary<HPController, float>();//valueは次の攻撃のタイミング
    private int damage;
    private string targetTag;
    private float attackInterval;

    public void InitAreaChild(string targetTag,float duration,int damage,float attackInterval)
    {
        this.targetTag = targetTag;
        this.damage = damage;
        this.attackInterval = attackInterval;
        Destroy(gameObject,duration);
    }

    private void Update() 
    {
        List<HPController> enemysDead = new List<HPController>();//foreachでCollectionをRemoveするとエラーが出るのを避けるため
        List<HPController> enemysDamaged = new List<HPController>();
        foreach(HPController target in targets.Keys)
        {
            //敵が倒されたときに呼ばれないようにする
            if(target == null)
            {
                enemysDead.Add(target);
                continue;
            }
            if(Time.time >= targets[target])
            {
                enemysDamaged.Add(target);
                target.Damaged(damage,target.gameObject.transform.position);
            }
        }

        foreach(HPController target in  enemysDamaged)targets[target] += attackInterval;

        HPController[] targetsContains = targets.Keys.ToArray();
        foreach(HPController target in targetsContains)
        {
            if(enemysDead.Contains(target))
            {
                targets.Remove(target);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(targetTag))
        {
            HPController hpController = other.GetComponentInParent<HPController>();

            // ゴーレムにはColliderが複数あり同じKeyで重複エラーになるので一度のみにする
            if (other.GetComponentInParent<Golem>() && targets.ContainsKey(hpController)) return;
            targets.Add(hpController, Time.time);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag(targetTag))
        {
            targets.Remove(other.GetComponentInParent<HPController>());
        }
    }
}
