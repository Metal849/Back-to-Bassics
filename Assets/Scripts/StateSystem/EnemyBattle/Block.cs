using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    public class Block : EnemyState
    {
        public override void AttackRequestHandler(IAttackRequester requester)
        {
            AnimatorStateInfo animatorState = Input.EnemySprite.Animator.GetCurrentAnimatorStateInfo(0);
            if (!animatorState.IsName("block") && !animatorState.IsName("blocking"))
            {
                Input.EnemySprite.Animator.Play("block");
            }
            else
            {
                Input.EnemySprite.Animator.Play("blocking");
            }
             
            //((PlayerBattlePawn)requester)?.Lurch(((EnemyBattlePawnData)Input.Enemy.Data).BlockLrch);
        }
        public override int OnDamage(int amount)
        {
            return (int)(amount * Input.Enemy.Data.BlockingReduction);
        }
        public override void Exit(EnemyStateInput i)
        {
            base.Exit(i);
            Input.EnemySprite.Animator.Play("idle");
        }
    }
}

