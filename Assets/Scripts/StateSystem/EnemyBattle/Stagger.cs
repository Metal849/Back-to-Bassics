using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    public class Stagger : EnemyState
    {
        public override void Enter(EnemyStateInput i)
        {
            base.Enter(i);
            Debug.Log("Staggered");
        }
        public override void Exit(EnemyStateInput i)
        {
            base.Exit(i);
            Debug.Log("UnStaggered");
        }
        public override void AttackRequestHandler(IAttackRequester requester)
        {
            //Input.EnemySprite.Animator.Play("take_damage");
        }
        public override int OnDamage(int amount)
        {
            return amount;
        }
    }
}

