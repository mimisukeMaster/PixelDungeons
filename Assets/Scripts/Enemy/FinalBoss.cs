using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : EnemyController
{
    public List<FinalBossChild> finalBossChildren = new List<FinalBossChild>();
    public HPController hPController;

    protected override void Start() 
    {
        base.Start();
        hPController.canBeDamaged = false;
        foreach(FinalBossChild finalBossChild in finalBossChildren)
        {
            finalBossChild.Init(this);
        }
    }

    protected override void Update() 
    {
        DetectPlayer();
    }

    public void OnChildDestroyed(FinalBossChild finalBossChild)
    {
        finalBossChildren.Remove(finalBossChild);
        if(finalBossChildren.Count <= 0)
        {
            OnAllChildrenDied();
        }
    }

    private void OnAllChildrenDied()
    {
        hPController.canBeDamaged = true;
    }

    private void Attack()
    {

    }

    public override void OnDied()
    {
        base.OnDied();

    }
}
