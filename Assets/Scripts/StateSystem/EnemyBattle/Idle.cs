using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    // Create two versions of Idle such that you can either slash at the opponet or not
    public class Idle : EnemyState
    {
        public override void AttackRequestHandler(IAttackRequester requester)
        {
            Input.Enemy.SpriteAnimator.Play("take_damage");
        }
        public override int OnDamage(int amount)
        {
            return amount;
        }
        public override float OnLurch(float amount)
        {
            return amount;
        }
    }
}
