using UnityEngine;

public class PlayerState : State
{
    protected Player player;
    protected Animator animator;
    
    public override void Initialize(StateMachine context, GameObject actor)
    {
        base.Initialize(context, actor);
        player = actor.GetComponent<Player>();
        animator = actor.GetComponent<Animator>();
    }
}
