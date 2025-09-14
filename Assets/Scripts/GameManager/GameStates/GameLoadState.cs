using UnityEngine;

public class GameLoadState : GameState
{
    public override void EnterState()
    {
        base.EnterState();
        while (!gameManager.SpawnPlayer())
        {
            gameManager.SpawnPlayer();
        }
        context.SwitchStates<GameDefaultState>(this);
    }
}
