public class WeaponFiringState : WeaponState
{
    protected string name = "FIRING";

    public override void EnterState()
    {
        weapon.ExecuteShoot();
        weapon.OnSwap += HandleSwap;
        weapon.OnShotFinished += OnShotFinishedHandler;
    }

    public override void ExitState()
    {
        weapon.OnSwap -= HandleSwap;
        weapon.OnShotFinished -= OnShotFinishedHandler;
    }

    private void HandleSwap()
    {
        context.SwitchStates<WeaponSwapState>(this);
    }

    private void OnShotFinishedHandler()
    {
        context.SwitchStates<WeaponReadyState>(this);
    }
}