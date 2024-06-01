using UnityEngine;

public class EnemyBattlePawn : BattlePawn, IAttackReceiver
{
    [Header("Enemy References")]
    [SerializeField] private EnemyStateMachine _esm;
    [SerializeField] private EnemyAction[] _enemyActions;
    [SerializeField] private int _beatsPerDecision;
    [SerializeField] private ParticleSystem _particleSystem;
    private float _decisionTime;
    private int _actionIdx;
    public EnemyStateMachine ESM => _esm;
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
        if (_enemyActions == null)
        {
            Debug.LogWarning("Enemy Battle Pawn has no actions referenced!");
            return;
        }
        foreach (EnemyAction action in _enemyActions)
        {
            if (action == null)
            {
                Debug.LogWarning("Enemy Battle Pawn has null action!");
            }
            action.ParentPawn = this;
        }
    }
    // Perform Random Battle Action
    protected override void OnFullBeat()
    {
        if (Conductor.Instance.Beat < _decisionTime || (_enemyActions != null && _enemyActions[_actionIdx].IsActive) || IsStaggered || IsDead || !ActiveBattlePawn) return;
        int idx = Random.Range(0, (_enemyActions != null ? _enemyActions.Length : 0) + 2) - 2;
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
        if (i >= _enemyActions.Length)
        {
            Debug.Log("Non Existent index call for batlle action!");
            return;
        }
        _esm.Transition<EnemyStateMachine.Attacking>();
        _enemyActions[i].StartAction();
        _actionIdx = i;
    }
    #region IAttackReceiver Methods
    public void ReceiveAttackRequest(IAttackRequester requester)
    {
        if (IsDead) return;
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
        if (IsStaggered) return;
        amount = _esm.CurrState.OnLurch(amount);
        base.Lurch(amount);
    }
    protected override void OnStagger()
    {
        if (IsDead) return;
        base.OnStagger();
        // Staggered Animation (Paper Crumple)
        _esm.Transition<EnemyStateMachine.Stagger>();
        _particleSystem?.Play();
    }
    protected override void OnUnstagger()
    {
        if (IsDead) return;
        base.OnUnstagger();
        // Unstagger Animation transition to idle
        _esm.Transition<EnemyStateMachine.Idle>();
        _particleSystem?.Stop();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        _esm.Transition<EnemyStateMachine.Dead>();
        BattleManager.Instance.OnEnemyDeath();
    }
    public void OnActionComplete()
    {
        _decisionTime = Conductor.Instance.Beat + _beatsPerDecision;
        if (IsStaggered || IsDead) return;
        _esm.Transition<EnemyStateMachine.Idle>();
    }
    #endregion
}
