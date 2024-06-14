using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
    public class Attacking : EnemyState
    {
        public override void AttackRequestHandler(IAttackRequester requester)
        {
            // Use VFX to show enemy taking dmg but unchange animation
        }
        public override int OnDamage(int amount)
        {
            return (int)(amount * Input.Enemy.Data.PreStaggerMultiplier);
        }
    }
}
