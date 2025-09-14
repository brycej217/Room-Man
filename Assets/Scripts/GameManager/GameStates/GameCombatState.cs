using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCombatState : GameState
{
    public override void EnterState()
    {
        base.EnterState();
        Player.OnDeath += OnDeathHandler;
        Debug.Log("combat entered");
    }

    public override void ExitState()
    {
        Player.OnDeath -= OnDeathHandler;
    }

    private void OnDeathHandler()
    {
        context.SwitchStates<GameDeathState>(this);
    }
}
