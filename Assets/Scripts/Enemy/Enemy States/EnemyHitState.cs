using UnityEngine;

public class EnemyHitState : EnemyState
{
    protected string name = "HIT";
    public override void EnterState()
    {
        base.EnterState();
        enemy.OnHit += HandleHit;
        enemy.OnStunFinished += OnStunExitHandler;
        GetHit();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.StopStun();
        enemy.OnStunFinished -= OnStunExitHandler;
        enemy.OnHit -= HandleHit;
    }

    private void GetHit()
    {
        switch (enemy.partHit)
        {
            case "Head":
                enemy.SubtractHealth(enemy.damageTaken * 2);
                Debug.Log("Critical hit");
                break;
            default:
                enemy.SubtractHealth(enemy.damageTaken);
                break;
        }
        if (enemy.currentHealth <= 0)
        {
            context.SwitchStates<EnemyDeathState>(this);
            return;

        }
        enemy.StartStun();
        animator.Play("HIT", 0, 0f);
        enemy.damageTaken = 0;
    }

    private void OnStunExitHandler()
    {
        if (!enemy.InRange())
        {
            context.SwitchStates<EnemyFollowState>(this);
        }
        else
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
