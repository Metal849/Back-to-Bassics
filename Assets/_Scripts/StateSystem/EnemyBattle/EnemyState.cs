using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    public abstract class EnemyState : State<EnemyStateMachine, EnemyState, EnemyStateInput>
    {
        public abstract bool AttackRequestHandler(IAttackRequester requester);
        public virtual int OnDamage(int amount) { return amount; }
    }
}

