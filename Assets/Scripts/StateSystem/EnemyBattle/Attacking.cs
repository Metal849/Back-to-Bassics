public partial class EnemyStateMachine
{
    public class Attacking : EnemyState
    {
        public override bool AttackRequestHandler(IAttackRequester requester)
        {
            // Use VFX to show enemy taking dmg but unchange animation
            return false;
        }
    }
}
