public interface IAttackRequester
{
    public void OnRequestDeflect(IAttackReceiver receiver);
    public void OnRequestBlock(IAttackReceiver receiver);
    public float AttackDamage { get; }
    public float AttackLurch { get; }
}
