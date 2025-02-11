using UnityEngine;

public class FinalBossChild : EnemyController
{
    private FinalBoss finalBoss;
    public void Init(FinalBoss finalBoss)
    {
        this.finalBoss = finalBoss;
    }

    protected override void Update()
    {
        
    }

    public override void OnDied(GameObject gameObject)
    {
        finalBoss.OnChildDestroyed(this);
        base.OnDied(gameObject);
    }
}
