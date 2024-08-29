public partial class PositionStateMachine
{
    public class Distant: PositionState
    {
        public override void Enter(PositionStateInput i)
        {
            base.Enter(i);
            Input.PawnSprite?.Animator.Play("step_back");
        }
        public override void Exit(PositionStateInput i)
        {
            base.Exit(i);
            Input.PawnSprite?.Animator.Play("step_forward");
        }
    }
}
