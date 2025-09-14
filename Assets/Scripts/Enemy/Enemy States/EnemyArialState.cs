public class EnemyArialState : EnemyState
{
    protected string name = "ARIAL";

    public override void UpdateState()
    {
        base.UpdateState();
        enemy.ApplyGravity(false);
    }

    public override void CheckSwitchStates()
    {
        if (enemy.IsGrounded())
        {
            context.SwitchStates<EnemyGroundedState>(this);
        }
    }
}
