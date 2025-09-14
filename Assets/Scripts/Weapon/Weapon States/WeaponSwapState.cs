using UnityEngine;

public class WeaponSwapState : WeaponState
{
    protected string name = "SWAP";

    public override void EnterState()
    {
        base.EnterState();
        weapon.Swap();
        weapon.OnSwap += SwapHandler;
        weapon.OnSwapFinished += SwapFinishedHandler;
    }

    public override void ExitState()
    {
        base.ExitState();
        weapon.StopSwap();
        weapon.OnSwap -= SwapHandler;
        weapon.OnSwapFinished -= SwapFinishedHandler;
    }

    private void SwapFinishedHandler()
    {
        context.SwitchStates<WeaponReadyState>(this);
    }

    private void SwapHandler()
    {
        context.SwitchStates<WeaponSwapState>(this);
    }
}
