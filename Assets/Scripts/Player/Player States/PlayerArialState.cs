using UnityEngine;

public class PlayerArialState : PlayerState
{
    protected string name = "ARIAL";

    public override void EnterState()
    {
        base.EnterState();
        player.movementSpeed = player.airSpeed;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        player.ApplyGravity(false);
    }

    public override void CheckSwitchStates()
    {
        if (player.IsGrounded())
        {
            context.SwitchStates<PlayerGroundedState>(this);
        }
    }
}
