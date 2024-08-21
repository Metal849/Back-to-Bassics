public interface IAttackRequester
{
    /// <summary>
    /// When the attack requested is challenged by the receiver
    /// </summary>
    /// <param name="receiver"></param>
    public void OnRequestDeflect(IAttackReceiver receiver);
    /// <summary>
    /// When the attack requeseted is denied by the receiver
    /// </summary>
    /// <param name="receiver"></param>
    public void OnRequestBlock(IAttackReceiver receiver);
    /// <summary>
    /// When the attack requested is avoided by the receiver
    /// </summary>
    /// <param name="receiver"></param>
    public void OnRequestDodge(IAttackReceiver receiver);
}
