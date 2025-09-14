using UnityEngine;

public class EnemyState : State
{
    protected Enemy enemy;
    protected Animator animator;

    public override void Initialize(StateMachine context, GameObject actor)
    {
        base.Initialize(context, actor);
        enemy = actor.GetComponent<Enemy>();
        animator = actor.GetComponent<Animator>();
    }
}
