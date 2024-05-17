public interface IAttackReceiver
{
    public abstract void ReceiveAttackRequest(IAttackRequester requester);
    /// <summary>
    /// Use this method when there is a timing between receiving the attack and completing the attack
    /// </summary>
    /// <param name="requester"></param>
    public abstract void CompleteAttackRequest(IAttackRequester requester);
}
