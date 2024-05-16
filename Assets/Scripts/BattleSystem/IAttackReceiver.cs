public interface IAttackReceiver
{
    public abstract void AttackRequest(IAttackRequester requester);
    public abstract void AttackComplete(IAttackRequester requester);
}
