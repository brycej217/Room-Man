public class WeaponReloadState : WeaponState
{
    protected string name = "RELOAD";

    public override void EnterState()
    {
        weapon.ExecuteReload();
        weapon.OnSwap += HandleSwap;
        weapon.OnReloadFinished += OnReloadFinishedHandler;
    }

    public override void ExitState()
    {
        weapon.OnSwap -= HandleSwap;
        weapon.OnReloadFinished -= OnReloadFinishedHandler;
    }

    private void HandleSwap()
    {
        context.SwitchStates<WeaponSwapState>(this);
    }

    private void OnReloadFinishedHandler()
    {
        context.SwitchStates<WeaponReadyState>(this);
    }
}
