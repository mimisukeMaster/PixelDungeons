using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class MiddleBossEnemy : EnemyController
{
    protected Animator animator;
    public AttackController attackController;

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        attackController.Init("Player", Attack);
    }
}
