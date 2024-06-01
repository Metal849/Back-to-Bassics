using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameStateMachine : StateMachine<GameStateMachine, GameStateMachine.GameState, GameStateInput>
{
    protected override void SetInitialState()
    {
        BattleManager.Instance.Player = GameManager.Instance.PC.GetComponent<PlayerBattlePawn>();
        Transition<WorldTraversal>();
    }
}
