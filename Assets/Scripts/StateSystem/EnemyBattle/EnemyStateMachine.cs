using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine : StateMachine<EnemyStateMachine, EnemyStateMachine.EnemyState, EnemyStateInput>
{
    #region Unity Messages
    protected override void Start()
    {
        base.Start();
        Conductor.Instance.OnBeat += OnBeat;
    }
    private void OnDestroy()
    {
        Conductor.Instance.OnBeat -= OnBeat;
    }
    #endregion
    protected override void SetInitialState()
    {
        Transition<Idle>();
    }
    private void OnBeat()
    {
        CurrState.ContextTransition();
    }
}
