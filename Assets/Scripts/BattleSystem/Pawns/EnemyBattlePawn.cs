using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Manipulate by an external class, not within the class!!
/// </summary>
public class EnemyBattlePawn : BattlePawn, IAttackReceiver
{
    [Header("Enemy References")]
    [SerializeField] private EnemyStateMachine _esm;
    // This will replace the need to reference enemy actions!
    [SerializeField] private TimelineAsset[] _enemyActionSequences;
    [SerializeField] private int _beatsPerDecision;
    //[SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private PlayableDirector _director;
    [field: SerializeField] public Transform targetFightingLocation { get; private set; }
    private float _decisionTime;
    public EnemyStateMachine ESM => _esm;
    public EnemyBattlePawnData EnemyData => (EnemyBattlePawnData)Data;
    private Dictionary<Type, EnemyAction> _enemyActions = new Dictionary<Type, EnemyAction>();
    protected override void Awake()
    {
        base.Awake();
        if (Data.GetType() != typeof(EnemyBattlePawnData))
        {
            Debug.LogError($"Enemy Battle Pawn \"{Data.name}\" is set incorrectly");
            return;
        }
        _esm = GetComponent<EnemyStateMachine>();
        _director = GetComponent<PlayableDirector>();
        if (_director == null)
        {
            Debug.LogError($"Enemy Battle Pawn \"{Data.name}\" has no playable director referenced!");
            return;
        }
    }
    public EA GetEnemyAction<EA>() where EA : EnemyAction
    {
        return _enemyActions[typeof(EA)] as EA;
    }
    public void AddEnemyAction(EnemyAction action)
    {
        if (_enemyActions.ContainsKey(action.GetType()))
        {
            Debug.LogError($"Enemy Action {action.GetType()} is redundantly referenced, only one should be.");
            return;
        }
        _enemyActions[action.GetType()] = action;
    }
    // Perform Random Battle Action --> This is not the way this should be done
    protected override void OnFullBeat()
    {
        // (Ryan) Should't need to check for death here, just disable the conducatable conductor connection 
        //if (Conductor.Instance.Beat < _decisionTime || (_enemyActions != null && _enemyActions[_actionIdx].IsActive) || IsDead) return;
        if (Conductor.Instance.Beat < _decisionTime || _director.state == PlayState.Playing || IsDead || IsStaggered) return;
        int idx = UnityEngine.Random.Range(0, (_enemyActionSequences != null ? _enemyActionSequences.Length : 0) + 2) - 2;
        //int idx = UnityEngine.Random.Range(0, 4);
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
            _esm.Transition<EnemyStateMachine.Attacking>();
            _director.playableAsset = _enemyActionSequences[idx];
            _director.Play();
            _director.playableGraph.GetRootPlayable(0).SetSpeed(1 / EnemyData.SPB);
        }
        _decisionTime = Conductor.Instance.Beat + _beatsPerDecision;
    }
    /// <summary>
    /// Select from some attack i to perform, and then provide a direction if the attack has variants based on this
    /// </summary>
    /// <param name="i"></param>
    /// <param name="dir"></param>
    //public void PerformBattleAction(int i)
    //{
    //    if (i >= _enemyActions.Length)
    //    {
    //        Debug.Log("Non Existent index call for batlle action!");
    //        return;
    //    }
    //    _esm.Transition<EnemyStateMachine.Attacking>();
    //    _enemyActions[i].StartAction();
    //    _actionIdx = i;
    //}
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
        if (_esm.IsOnState<EnemyStateMachine.Idle>())
        {
            _esm.Transition<EnemyStateMachine.Block>();
        }
    }
    //public override void Lurch(float amount)
    //{
    //    if (IsStaggered) return;
    //    amount = _esm.CurrState.OnLurch(amount);
    //    base.Lurch(amount);
    //}
    protected override void OnStagger()
    {
        if (IsDead) return;
        base.OnStagger();
        // Staggered Animation (Paper Crumple)
        _esm.Transition<EnemyStateMachine.Stagger>();
        //_particleSystem?.Play();
    }
    protected override void OnUnstagger()
    {
        if (IsDead) return;
        base.OnUnstagger();
        // Unstagger Animation transition to idle
        _esm.Transition<EnemyStateMachine.Idle>();
        //_particleSystem?.Stop();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        _director.Stop();
        _esm.Transition<EnemyStateMachine.Dead>();
        //_particleSystem?.Stop();
    }
    // This could get used or not, was intended for ranom choices :p
    public void OnActionComplete()
    {
        _decisionTime = Conductor.Instance.Beat + _beatsPerDecision;
        if (IsDead || IsStaggered) return;
        _esm.Transition<EnemyStateMachine.Idle>();
    }
    #endregion
}
