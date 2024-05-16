using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
public class BattlePawn : Conductable, IAttackReceiver
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
    protected IAttackRequester _activeAttackRequester;
    #region Unity Messages
    protected virtual void Awake()
    {
        _currHP = _data.HP;
        _currSP = _data.SP;
        gameObject.SetActive(false);
    }
    public void FixedUpdate()
    {
        if (_currSP < _data.SP)
        {
            _currSP += _data.StaggerRecoveryRate * Time.deltaTime;
            _spBar.fillAmount = _currSP / _data.SP;
        }
    }
    #endregion
    public void Damage(int amount)
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
    public void Lurch(int amount)
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
    public void EnterBattle()
    {
        gameObject.SetActive(true);
        _spriteAnimator.Play("EnterBattle");
    }
    public void LeaveBattle()
    {
        // TODO: Play Some Animation that makes the battle pawn leave the battle
    }
    protected virtual void OnStagger()
    {
        // TODO: Things that occur on battle pawn stagger
    }
    protected virtual void OnDeath()
    {
        // TODO: Things that occur on battle pawn death
    }
    #region IAttackRequester Methods
    public void AttackRequest(IAttackRequester requester)
    {
        _activeAttackRequester = requester;
    }

    public void AttackComplete(IAttackRequester requester)
    {
        if (_activeAttackRequester != requester)
        {
            Debug.Log("Attack Request and Completion missmatch, expected attack requester \"" + _activeAttackRequester + "\" instead got \"" + requester +".\"");
            return;
        }
        _activeAttackRequester = null;
    }
    #endregion
}
