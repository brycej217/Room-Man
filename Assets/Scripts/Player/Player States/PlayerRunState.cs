using UnityEngine;

public class PlayerRunState : PlayerState
{
    protected string name = "RUN";
    private Vector2 movementInput;

    public override void EnterState()
    {
        base.EnterState();
        animator.Play("RUN");
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
        if (player.GetSpeed() <= 0.1)
        {
            context.SwitchStates<PlayerIdleState>(this);
        }
    }
}
