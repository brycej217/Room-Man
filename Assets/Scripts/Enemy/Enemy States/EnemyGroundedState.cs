public class EnemyGroundedState : EnemyState
{
    protected string name = "GROUNDED";
    public override void UpdateState()
    {
        base.UpdateState();
        enemy.ApplyGravity(true);
    }

    public override void CheckSwitchStates()
    {
        if (!enemy.IsGrounded())
        {
            context.SwitchStates<EnemyArialState>(this);
        }
    }
}
