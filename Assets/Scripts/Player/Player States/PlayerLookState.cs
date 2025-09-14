using UnityEngine;

public class PlayerLookState : PlayerState
{
    protected string name = "LOOK";
    private Vector2 lookInput;

    public override void UpdateState()
    {
        base.UpdateState();

        // looking
        lookInput = player.lookAction.ReadValue<Vector2>();
        player.Look(lookInput);

        // weapon
        if (player.shootAction.IsPressed())
        {
            player.Shoot();
        }

        if (player.reloadAction.IsPressed())
        {
            player.Reload();
        }

        // swap weapon
        float swapValue = player.swapAction.ReadValue<float>();
        if (swapValue != 0)
        {
            player.SwapWeapon(swapValue);
        }
    }
}
