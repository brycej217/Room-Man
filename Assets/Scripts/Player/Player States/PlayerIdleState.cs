using UnityEngine;

public class PlayerIdleState : PlayerState
{
    protected string name = "IDLE";
    private Vector2 movementInput;

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        movementInput = player.moveAction.ReadValue<Vector2>();

    }

    public override void FixedUpdateState()
    {
        player.Move(movementInput);
    }

    public override void CheckSwitchStates()
    {
        if (player.GetSpeed() > 0.1)
        {
            context.SwitchStates<PlayerRunState>(this);
        }
    }
}
