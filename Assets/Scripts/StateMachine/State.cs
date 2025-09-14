using UnityEngine;

[System.Serializable]
public class State
{
    protected StateMachine context;
    protected GameObject actor;

    public virtual void Initialize(StateMachine context, GameObject actor)
    {
        this.context = context;
        this.actor = actor;
    }

    public virtual void EnterState()
    {
        //Debug.Log($"Entering {this.name} State");
    }

    public virtual void ExitState()
    {
        //Debug.Log($"Exiting {this.name} State");
    }

    public virtual void UpdateState()
    {
        CheckSwitchStates();
    }

    public virtual void FixedUpdateState()
    {
        
    }

    public virtual void CheckSwitchStates()
    {

    }
}
