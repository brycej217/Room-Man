using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadyState : PlayerState
{
    protected string name = "READY";
    
    public override void UpdateState()
    {
        base.UpdateState();

        // melee
        if (player.meleeAction.WasPressedThisFrame())
        {
            context.SwitchStates<PlayerMeleeState>(this);
        }

        // lasso
        if (player.lassoAction.WasPressedThisFrame())
        {
            context.SwitchStates<PlayerLassoState>(this);
        }
    }
}
