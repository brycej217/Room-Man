using UnityEngine;

public class EnemyAttackState : EnemyState
{
    protected string name = "ATTACK";
    private static readonly int AttackHash = Animator.StringToHash("ATTACK");
    private float animationStartTime;

    public override void EnterState()
    {
        base.EnterState();
        animator.Play(AttackHash);
        animationStartTime = Time.time;
        enemy.OnHit += HandleHit;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.OnHit -= HandleHit;
    }

    public override void CheckSwitchStates()
    {
        if (IsAnimationFinished(AttackHash))
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
    }

    private void HandleHit(float amount, string tag)
    {
        enemy.damageTaken = amount;
        enemy.partHit = tag;
        context.SwitchStates<EnemyHitState>(this);
    }

    private bool IsAnimationFinished(int animationHash)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash == animationHash)
        {
            AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfos.Length > 0)
            {
                float clipLength = clipInfos[0].clip.length;
                float elapsedTime = Time.time - animationStartTime;
                return elapsedTime >= clipLength;
            }
        }
        return false;
    }
}