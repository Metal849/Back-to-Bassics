using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameStateMachine;

public abstract class Combo : ScriptableObject
{
    [SerializeField] private string strId;
    [SerializeField] private int cost;
    public int Cost => cost;
    public string StrId => strId;
    public abstract void InBattle();
    public abstract void InTraversal();
    public void StartComboAttack()
    {
        if (GameManager.Instance.GSM.IsOnState<Battle>())
        {
            InBattle();
        }
        else if (GameManager.Instance.GSM.IsOnState<WorldTraversal>())
        {
            InTraversal();
        }
    }
}
