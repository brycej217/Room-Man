using UnityEngine;

public class GameDeathState : GameState
{
    public override void EnterState()
    {
        base.EnterState();
        Debug.LogWarning("Player has died");
        Time.timeScale = 0f;
    }
}
