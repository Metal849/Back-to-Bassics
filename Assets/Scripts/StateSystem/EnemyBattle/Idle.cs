using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyStateMachine
{
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

        public override void ContextTransition()
        {
            
        }
    }
}
