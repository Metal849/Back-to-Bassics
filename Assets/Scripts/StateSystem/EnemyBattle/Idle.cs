using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    // Create two versions of Idle such that you can either slash at the opponet or not
    public class Idle : EnemyState
    {
        public override void Enter(EnemyStateInput i)
        {
            base.Enter(i);
            //Input.Enemy.SpriteAnimator.Play("Idle");
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit(EnemyStateInput i)
        {
            base.Exit(i);
        }

        public override void AttackRequestHandler(IAttackRequester requester)
        {
            Input.Enemy.Damage(requester.AttackDamage);
            Input.Enemy.Lurch(requester.AttackLurch);
            //Input.Enemy.CompleteAttackRequest(requester);
        }
    }
}
