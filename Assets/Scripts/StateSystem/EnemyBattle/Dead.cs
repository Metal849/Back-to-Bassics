using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    public class Dead : EnemyState
    {
        public override void AttackRequestHandler(IAttackRequester requester)
        {
            throw new System.NotImplementedException();
        }

        public override void Enter(EnemyStateInput i)
        {
            base.Enter(i);
            Input.EnemySprite.Animator.Play("dead");
        }

        public override int OnDamage(int amount)
        {
            return 0;
        }
    }
}

