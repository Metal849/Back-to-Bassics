using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[DisallowMultipleComponent]
public class BattlePawn : Conductable
{
    [Header("References")]
    [SerializeField] protected BattlePawnData _data;
    [SerializeField] protected Animator _spriteAnimator;
    public BattlePawnData Data => _data;
    public Animator SpriteAnimator => _spriteAnimator;

    [Header("Battle Pawn Data")]
    [SerializeField] protected int _currHP;
    [SerializeField] protected float _currSP;
    public int HP => _currHP;
    public float SP => _currSP;

    #region BattlePawn Boolean States
    public bool IsDead { get; private set; }
    public bool IsStaggered { get; private set; }
    #endregion

    #region Unity Messages
    protected virtual void Awake()
    {
        _currHP = _data.HP;
        _currSP = _data.SP;
    }
    #endregion
    #region Conductable Methods
    protected override void OnHalfBeat()
    {
        if (IsStaggered) return;
        RecoverSP(_data.SPRecoveryRate);
    }
    #endregion
    #region Modification Methods
    public virtual void Damage(int amount)
    {
        if (IsDead) return;
        _currHP -= amount;
        UIManager.Instance.UpdateHP(this);
        if (_currHP <= 0) 
        {
            // Battle Pawn Death
            _currHP = 0;
            IsDead = true;
            // TODO: Play Death Animation
            // TODO: Notify BattleManager to broadcast this BattlePawn's death
            OnDeath();
        }
    }
    public virtual void Lurch(float amount)
    {
        if (IsStaggered) return;
        _currSP -= amount;
        UIManager.Instance.UpdateSP(this);
        if (_currSP <= 0)
        {
            // Battle Pawn Stagger
            _currSP = 0;
            StartCoroutine(StaggerSelf());
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
    public virtual void RecoverSP(float amount)
    {
        if (_currSP < _data.SP)
        {
            // Updates integer wise
            _currSP += amount;
            UIManager.Instance.UpdateSP(this);
        }
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
        gameObject.SetActive(true);
        //_spriteAnimator.Play("enterbattle");
    }
    public virtual void LeaveBattle()
    {
        // TODO: Play Some Animation that makes the battle pawn leave the battle
    }
    #region BattlePawn Messages
    protected virtual void OnStagger()
    {
        // TODO: Things that occur on battle pawn stagger
    }
    protected virtual void OnDeath()
    {
        // TODO: Things that occur on battle pawn death
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
        yield return new WaitForSeconds(_data.StaggerRecoveryTime);
        _currSP = _data.SP;
        UIManager.Instance.UpdateSP(this);
        IsStaggered = false;
        OnUnstagger();
        // TODO: Play StaggerRecovery Animation
    }
}
