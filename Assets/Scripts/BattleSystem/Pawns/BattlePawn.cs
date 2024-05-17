using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattlePawn : Conductable
{
    [Header("References")]
    [SerializeField] protected BattlePawnData _data;
    [SerializeField] protected Animator _spriteAnimator;

    [Header("Battle Pawn Data")]
    [SerializeField] protected float _currHP;
    [SerializeField] protected float _currSP;

    [Header("Temporary Direct Ref UI tracking")]
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _spBar;
    public float HP => _currHP;
    public float SP => _currSP;
    public BattlePawnData Data => _data;
    public bool IsDead { get; private set; }
    public bool IsStaggered { get; private set; }
    public Animator SpriteAnimator => _spriteAnimator;
    #region Unity Messages
    protected virtual void Awake()
    {
        _currHP = _data.HP;
        _currSP = _data.SP;
        gameObject.SetActive(false);
    }
    public void FixedUpdate()
    {
        RecoverSP(_data.StaggerRecoveryRate * Time.deltaTime);
    }
    #endregion
    public virtual void Damage(float amount)
    {
        if (IsDead) return;
        _currHP -= amount;
        if (_hpBar != null) _hpBar.fillAmount = _currHP / _data.HP;
        if (_currHP <= 0) 
        {
            IsDead = true;
            _currHP = 0;
            OnDeath();
        }
    }
    public virtual void Lurch(float amount)
    {
        if (IsStaggered) return;
        _currSP -= amount;
        if (_spBar != null) _spBar.fillAmount = _currSP / _data.SP;
        if (_currSP <= 0)
        {
            //IsStaggered = true; UNCOMMENT ME WHEN THERE IS A WAY TO UNSTAGGER
            _currSP = 0;
            OnStagger();
        }
    }
    public virtual void RecoverSP(float amount)
    {
        if (_currSP < _data.SP)
        {
            _currSP += amount;

            // TEMP UGLY UI UPDATING
            _spBar.fillAmount = _currSP / _data.SP;
        }
    }
    public virtual void EnterBattle()
    {
        gameObject.SetActive(true);
        _spriteAnimator.Play("EnterBattle");
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
    #endregion
}
