using UnityEngine;

public class GameState : State
{
    protected GameManager gameManager;

    public override void Initialize(StateMachine context, GameObject actor)
    {
        base.Initialize(context, actor);
        gameManager = actor.GetComponent<GameManager>();
    }
}
