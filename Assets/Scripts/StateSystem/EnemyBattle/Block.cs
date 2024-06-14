using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    public class Block : EnemyState
    {
        public override void AttackRequestHandler(IAttackRequester requester)
        {
            AnimatorStateInfo animatorState = Input.Enemy.SpriteAnimator.GetCurrentAnimatorStateInfo(0);
            if (!animatorState.IsName("block") && !animatorState.IsName("blocking"))
            {
                Input.Enemy.SpriteAnimator.Play("block");
            }
            else
            {
                Input.Enemy.SpriteAnimator.Play("blocking");
            }
             
            //((PlayerBattlePawn)requester)?.Lurch(((EnemyBattlePawnData)Input.Enemy.Data).BlockLrch);
        }
        public override int OnDamage(int amount)
        {
            return 0;
        }
        public override void Exit(EnemyStateInput i)
        {
            base.Exit(i);
            Input.Enemy.SpriteAnimator.Play("idle");
        }
    }
}

