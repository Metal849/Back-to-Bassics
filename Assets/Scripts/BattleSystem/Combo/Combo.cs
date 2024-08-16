using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combo : ScriptableObject
{
    [SerializeField] private string strId;
    [SerializeField] private int cost;
    public int Cost => cost;
    public string StrId => strId;
    public abstract void DoCombo();
}
