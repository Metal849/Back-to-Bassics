using UnityEngine;

public class EnemyBattlePawn : BattlePawn, IAttackReceiver
{
    [Header("Enemy References")]
    [SerializeField] private EnemyStateMachine _esm;
    [SerializeField] private EnemyAction[] _enemyActions;
    [SerializeField] private int _beatsPerDecision;
    private float _decisionTime;
    private EnemyAction _activeAction;
    protected override void Awake()
    {
        base.Awake();
        if (Data.GetType() != typeof(EnemyBattlePawnData))
        {
            Debug.LogError("Enemy Battle Pawn is set incorrectly");
            return;
        }
        _esm = GetComponent<EnemyStateMachine>();

        // Attacks Shouldn't be instantiated, they should come bundled with the enemy prefab!! Its cleaner and more efficient!
        if (_enemyActions == null) return;
        foreach (EnemyAction action in _enemyActions)
        {
            action.ParentPawn = this;
        }
    }
    // Perform Random Battle Action
    protected override void OnFullBeat()
    {
        if (Conductor.Instance.Beat < _decisionTime || !_esm.IsOnState<EnemyStateMachine.Attacking>() || IsStaggered) return;
        int idx = Random.Range(0, _enemyActions.Length + 2) - 2;
        if (idx == -2)
        {
            _esm.Transition<EnemyStateMachine.Idle>();
        }
        else if (idx == -1)
        {
            _esm.Transition<EnemyStateMachine.Block>();
        }
        else
        {
            PerformBattleAction(idx);
        }
        _decisionTime = Conductor.Instance.Beat + _beatsPerDecision;
    }
    /// <summary>
    /// Select from some attack i to perform, and then provide a direction if the attack has variants based on this
    /// </summary>
    /// <param name="i"></param>
    /// <param name="dir"></param>
    public void PerformBattleAction(int i)
    {
        _esm.Transition<EnemyStateMachine.Attacking>();
        _enemyActions[i].StartAction();
        _activeAction = _enemyActions[i];
    }
    #region IAttackReceiver Methods
    public void ReceiveAttackRequest(IAttackRequester requester)
    {
        _esm.CurrState.AttackRequestHandler(requester);
    }

    public void CompleteAttackRequest(IAttackRequester requester)
    {
        throw new System.NotImplementedException();
    }
    #endregion
    #region BattlePawn Overrides
    public override void Damage(int amount)
    {
        amount = _esm.CurrState.OnDamage(amount);
        base.Damage(amount);
    }
    public override void Lurch(float amount)
    {
        amount = _esm.CurrState.OnLurch(amount);
        base.Lurch(amount);
    }
    #endregion
}
