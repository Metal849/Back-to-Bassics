using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine : StateMachine<EnemyStateMachine, EnemyStateMachine.EnemyState, EnemyStateInput>
{
    #region Unity Messages
    #endregion
    protected override void SetInitialState()
    {
        CurrInput.Enemy = GetComponent<EnemyBattlePawn>();
        Transition<Idle>();   
    }
}
