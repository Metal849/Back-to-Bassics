using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    public class Block : EnemyState
    {
        public override void AttackRequestHandler(IAttackRequester requester)
        {
            Input.Enemy.SpriteAnimator.Play("block");
            ((PlayerBattlePawn)requester)?.Lurch(((EnemyBattlePawnData)Input.Enemy.Data).BlockLrch);
        }
        public override int OnDamage(int amount)
        {
            return 0;
        }
        public override float OnLurch(float amount)
        {
            return 0;
        }
    }
}

