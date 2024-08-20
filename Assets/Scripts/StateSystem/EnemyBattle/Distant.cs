using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    // Create two versions of Idle such that you can either slash at the opponet or not
    public class Distant: EnemyState
    {
        public override void Enter(EnemyStateInput i)
        {
            base.Enter(i);
            Input.EnemySprite.Animator.Play("step_back");
        }
        public override void Exit(EnemyStateInput i)
        {
            base.Exit(i);
            Input.EnemySprite.Animator.Play("center");
        }
        public override void AttackRequestHandler(IAttackRequester requester)
        {
            //Input.EnemySprite.Animator.Play("take_damage");
        }
        public override int OnDamage(int amount)
        {
            return 0;
        }
    }
}
