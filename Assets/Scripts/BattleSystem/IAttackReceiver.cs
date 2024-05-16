public interface IAttackReceiver
{
    public abstract void ReceiveAttackRequest(IAttackRequester requester);
    public abstract void CompleteAttackRequest(IAttackRequester requester);
}
