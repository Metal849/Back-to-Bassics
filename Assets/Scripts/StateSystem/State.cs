using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    private string stateName;
    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void ExitState() { }
    public abstract void CheckSwitchState();
}
