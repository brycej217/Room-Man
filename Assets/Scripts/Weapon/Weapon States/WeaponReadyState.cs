using UnityEngine;

public class WeaponReadyState : WeaponState
{
    protected string name = "READY";

    public override void EnterState()
    {
        base.EnterState();
        weapon.OnSwap += HandleSwap;
        weapon.OnShoot += HandleShoot;
        weapon.OnReload += HandleReload;
    }

    public override void ExitState()
    {
        weapon.OnSwap -= HandleSwap;
        weapon.OnShoot -= HandleShoot;
        weapon.OnReload -= HandleReload;
    }

    private void HandleSwap()
    {
        context.SwitchStates<WeaponSwapState>(this);
    }

    private void HandleShoot()
    {
        if (weapon.currentClipSize > 0)
        {
            context.SwitchStates<WeaponFiringState>(this);
        }
    }

    private void HandleReload()
    {
        context.SwitchStates<WeaponReloadState>(this);
    }
}
