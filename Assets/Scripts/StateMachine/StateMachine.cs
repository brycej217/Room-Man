using System;
using System.Collections.Generic;
using UnityEngine;

public enum StateMachineType
{
    PLAYER,
    ENEMY,
    WEAPON,
    GAME
}

public class StateMachine : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private StateMachineType type;

    [Header("States")]
    private List<State> states;

    private void Awake()
    {
        states = new List<State>();
        EnterDefaultStates();
    }

    #region State Management

    private void Update()
    {
        UpdateStates();
    }

    private void FixedUpdate()
    {
        FixedUpdateStates();
    }

    private void UpdateStates()
    {
        for (int i = 0; i < states.Count; i++)
        {
            states[i].UpdateState();
        }
    }

    private void FixedUpdateStates()
    {
        for (int i = 0; i < states.Count; i++)
        {
            states[i].FixedUpdateState();
        }
    }

    #endregion

    #region State Switching

    private void EnterDefaultStates()
    {
        switch (type)
        {
            case StateMachineType.PLAYER:
                SwitchStates<PlayerIdleState>(null);
                SwitchStates<PlayerArialState>(null);
                SwitchStates<PlayerReadyState>(null);
                SwitchStates<PlayerLookState>(null);
                break;
            case StateMachineType.ENEMY:
                SwitchStates<EnemyLoadState>(null);
                break;
            case StateMachineType.WEAPON:
                SwitchStates<WeaponReadyState>(null);
                break;
            case StateMachineType.GAME:
                SwitchStates<GameLoadState>(null);
                break;
            default:
                Debug.LogWarning("Unhandled State Machine Type: " + type);
                break;
        }
    }

    public void SwitchStates<T>(State oldState) where T : State, new()
    {
        if (oldState != null)
        {
            oldState.ExitState();
            states.Remove(oldState);
        }
        AddState(new T());
    }

    private void AddState(State stateInstance)
    {
        stateInstance.Initialize(this, gameObject);
        stateInstance.EnterState();
        states.Add(stateInstance);
    }

    #endregion

}