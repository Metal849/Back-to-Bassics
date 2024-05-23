using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    public class Stagger : EnemyState
    {
        public override void AttackRequestHandler(IAttackRequester requester)
        {
            //Input.Enemy.SpriteAnimator.Play("take_damage");
        }
        public override int OnDamage(int amount)
        {
            return amount;
        }
        public override float OnLurch(float amount)
        {
            return 0;
        }
        public override void Exit(EnemyStateInput i)
        {
            base.Exit(i);
            //Input.Enemy.OnUnstagger();
        }
    }
}

