using UnityEngine;

public class MiddleBossEnemy : EnemyController
{
    protected Animator animator;
    public AttackController attackController;

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        attackController.Init("Player", Attack, float.PositiveInfinity);
    }
}
