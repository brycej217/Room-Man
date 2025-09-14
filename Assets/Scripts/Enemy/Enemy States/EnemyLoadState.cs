public class EnemyLoadState : EnemyState
{
    protected string name = "FOLLOW";

    public override void EnterState()
    {
        base.EnterState();
        enemy.OnLoadFinished += OnLoadFinishedHandler;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.OnLoadFinished -= OnLoadFinishedHandler;
    }

    private void OnLoadFinishedHandler()
    {
        context.SwitchStates<EnemyFollowState>(this);
    }
}