using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLassoState : PlayerState
{
    protected string name = "LASSO";
    
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Lasso");
    }
}
