using UnityEngine;

public class GameDefaultState : GameState
{
    public override void EnterState()
    {
        base.EnterState();
        Player.OnDeath += OnDeathHandler;
        Location.OnLocationEnter += OnLocationEnterHandler;
    }

    public override void ExitState()
    {
        Player.OnDeath -= OnDeathHandler;
        Location.OnLocationEnter -= OnLocationEnterHandler;
    }

    private void OnDeathHandler()
    {
        context.SwitchStates<GameDeathState>(this);
    }

    private void OnLocationEnterHandler()
    {
        context.SwitchStates<GameCombatState>(this);
    }
}
