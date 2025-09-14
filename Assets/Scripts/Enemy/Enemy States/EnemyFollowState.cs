using UnityEngine;

public class EnemyFollowState : EnemyState
{
    protected string name = "FOLLOW";

    public override void EnterState()
    {
        base.EnterState();
        enemy.StartFollow(Player.Instance.transform);
        animator.Play("FOLLOW");
        enemy.OnHit += HandleHit;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.StopFollow();
        enemy.OnHit -= HandleHit;
    }

    public override void CheckSwitchStates()
    {
        if (enemy.InRange())
        {
            context.SwitchStates<EnemyAttackState>(this);
        }
    }

    private void HandleHit(float amount, string tag)
    {
        enemy.damageTaken = amount;
        enemy.partHit = tag;
        context.SwitchStates<EnemyHitState>(this);
    }
}
