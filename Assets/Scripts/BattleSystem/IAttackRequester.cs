public interface IAttackRequester
{
    public void OnRequestDeflect(IAttackReceiver receiver);
    public void OnRequestBlock(IAttackReceiver receiver);
    public float AttackDamage()
    {
        return 0;
    }
    public float AttackLurch()
    {
        return 0;
    }
}
