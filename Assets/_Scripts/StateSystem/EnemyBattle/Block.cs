using UnityEngine;
public partial class EnemyStateMachine
{
    public class Block : EnemyState
    {
        public override bool AttackRequestHandler(IAttackRequester requester)
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

            return false;
        }
    }
}

