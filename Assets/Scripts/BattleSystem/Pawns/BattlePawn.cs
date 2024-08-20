using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[DisallowMultipleComponent]
public class BattlePawn : Conductable
{
    [Header("References")]
    [SerializeField] private BattlePawnData _data;
    public BattlePawnData Data => _data;
    protected Animator _pawnAnimator;
    protected PawnSprite _pawnSprite;

    [Header("Battle Pawn Data")]
    [SerializeField] protected int _currHP;
    public int HP => _currHP;

    #region BattlePawn Boolean States
    public bool IsDead { get; private set; }
    public bool IsStaggered { get; private set; }
    #endregion

    // events
    public event Action OnPawnDeath;
    public event Action OnEnterBattle;
    public event Action OnExitBattle;
    public event Action OnDamage;

    #region Unity Messages
    protected virtual void Awake()
    {
        _currHP = _data.HP;
        _pawnAnimator = GetComponent<Animator>();
        _pawnSprite = GetComponentInChildren<PawnSprite>();
    }
    #endregion
    #region Modification Methods
    public virtual void Damage(int amount)
    {
        if (IsDead) return;
        _currHP -= amount;
        UIManager.Instance.UpdateHP(this);
        OnDamage?.Invoke();
        if (_currHP <= 0) 
        {
            // Battle Pawn Death
            _currHP = 0;
            IsDead = true;
            // Handling of Death animation and battlemanger Broadcast happen in OnDeath()
            BattleManager.Instance.OnPawnDeath(this);
            OnDeath();
        }
    }
    public virtual void Heal(int amount)
    {
        if (_currHP < _data.HP)
        {
            _currHP += amount;
            UIManager.Instance.UpdateHP(this);
        }
    }
    public virtual void Stagger()
    {
        StartCoroutine(StaggerSelf());
    }
    // TODO: Implement Status Ailment Applier Method
    // This should just be in the form of a GameObject Component
    public virtual void ApplyStatusAilments(StatusAilment[] ailments)
    {
        // (For Future Programmer) Code template when I actually implement everything
        //foreach (StatusAilment ailment in ailments)
        //{
        //    var ailmentComponent = GetComponent<ailment.type>();
        //    if (ailmentComponent == null)
        //    {
        //        ailmentComponent = gameObject.AddComponent<ailment.type>();
        //    }
        //    ailmentComponent.BuildUp(ailment.strength);
        //}
    }

    #endregion
    public virtual void EnterBattle()
    {
        Enable();
        OnEnterBattle?.Invoke();
        UIManager.Instance.UpdateHP(this);
        //_spriteAnimator.Play("enterbattle");
    }
    public virtual void ExitBattle()
    {
        // TODO: Play Some Animation that makes the battle pawn leave the battle
        Disable();
        OnExitBattle?.Invoke();
    }
    #region BattlePawn Messages
    protected virtual void OnStagger()
    {
        // TODO: Things that occur on battle pawn stagger
    }
    protected virtual void OnDeath()
    {
        // TODO: Things that occur on battle pawn death
        OnPawnDeath?.Invoke();
    }
    protected virtual void OnUnstagger()
    {
        // TODO: Things that occur on battle pawn after unstaggering
    }
    #endregion
    protected virtual IEnumerator StaggerSelf()
    {
        IsStaggered = true;
        OnStagger();
        // TODO: Play Stagger Animation
        // TODO: Notify BattleManager to broadcast this BattlePawn's stagger
        yield return new WaitForSeconds(_data.StaggerDuration);
        //_currSP = _data.SP;
        //UIManager.Instance.UpdateSP(this);
        IsStaggered = false;
        OnUnstagger();
        // TODO: Play StaggerRecovery Animation
    }
}
