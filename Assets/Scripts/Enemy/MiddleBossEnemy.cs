using UnityEngine;

public class MiddleBossEnemy : EnemyController
{
    protected Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
    }
}
