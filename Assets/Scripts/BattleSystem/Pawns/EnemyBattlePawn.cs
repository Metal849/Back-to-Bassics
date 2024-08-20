using System;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateMachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Manipulate by an external class, not within the class!!
/// </summary>
public class EnemyBattlePawn : BattlePawn, IAttackReceiver
{
    [field: Header("Enemy References")]
    [field: SerializeField] public EnemyStateMachine esm { get; private set; }
    //[SerializeField] private ParticleSystem _particleSystem;
    [field: SerializeField] public Transform targetFightingLocation { get; private set; }
    public EnemyBattlePawnData EnemyData => (EnemyBattlePawnData)Data;
    private Dictionary<Type, EnemyAction> _enemyActions = new Dictionary<Type, EnemyAction>();
    public event Action OnEnemyActionComplete;
    protected override void Awake()
    {
        base.Awake();
        if (Data.GetType() != typeof(EnemyBattlePawnData))
        {
            Debug.LogError($"Enemy Battle Pawn \"{Data.name}\" is set incorrectly");
            return;
        }
        esm = GetComponent<EnemyStateMachine>();
        Transform enemyActionsLocation = transform.Find("enemy_actions");
        if (enemyActionsLocation == null) 
        {
            Debug.LogError($"Enemy Battle Pawn \"{Data.name}\" must have child \"enemy_actions\"");
            return;
        }
        foreach (Transform child in enemyActionsLocation) 
        {
            var ea = child.GetComponent<EnemyAction>();
            if (ea == null) continue;
            AddEnemyAction(ea);
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
        esm.CurrState.AttackRequestHandler(requester);
    }
    public void CompleteAttackRequest(IAttackRequester requester)
    {
        throw new System.NotImplementedException();
    }
    #endregion
    #region BattlePawn Overrides
    public override void Damage(int amount)
    {
        amount = esm.CurrState.OnDamage(amount);
        base.Damage(amount);  
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
        esm.Transition<Stagger>();
        //_particleSystem?.Play();
    }
    protected override void OnUnstagger()
    {
        if (IsDead) return;
        base.OnUnstagger();
        // Unstagger Animation transition to idle
        esm.Transition<Idle>();
        //_particleSystem?.Stop();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        esm.Transition<Dead>();
        //_particleSystem?.Stop();
    }
    // This could get used or not, was intended for ranom choices :p
    public void OnActionComplete()
    {
        //OnEnemyActionComplete.Invoke();
        //if (IsDead || IsStaggered) return;
        //esm.Transition<Idle>();
    }
    #endregion
}
