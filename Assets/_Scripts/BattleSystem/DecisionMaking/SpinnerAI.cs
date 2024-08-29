using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerAI : Conductable
{
    private EnemyBattlePawn _spinner;
    private void Awake()
    {
        _spinner = GetComponent<EnemyBattlePawn>();
    }
    private void Start()
    {
        _spinner.OnEnterBattle += delegate { _spinner.GetEnemyAction<RotationAction>().StartAction(); };
        _spinner.OnExitBattle += delegate { _spinner.GetEnemyAction<RotationAction>().StopAction(); };
    }
}
