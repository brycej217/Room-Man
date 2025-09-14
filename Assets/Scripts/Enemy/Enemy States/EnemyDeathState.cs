public class EnemyDeathState : EnemyState
{
    protected string name = "DEATH";
    public override void EnterState()
    {
        base.EnterState();
        animator.Play("DEATH");
    }
}
