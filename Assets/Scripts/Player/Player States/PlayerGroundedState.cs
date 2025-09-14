public class PlayerGroundedState : PlayerState
{
    protected string name = "GROUNDED";

    public override void EnterState()
    {
        base.EnterState();
        player.movementSpeed = player.groundSpeed;
        animator.Play("STOP JUMP");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        player.ApplyGravity(true);
        if (player.jumpAction.IsPressed())
        {
            player.Jump();
        }
    }

    public override void CheckSwitchStates()
    {
        if (!player.IsGrounded())
        {
            context.SwitchStates<PlayerArialState>(this);
        }
    }
}
