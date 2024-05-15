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
    public Animator SpriteAnimator => _spriteAnimator;

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
    public void Damage(int amount)
    {
        _currHP -= amount;
        if (_hpBar != null) _hpBar.fillAmount = _currHP / _data.HP;
        if (_currHP < 0) _currHP = 0;
    }
    public void Lurch(int amount)
    {
        _currSP -= amount;
        if (_spBar != null) _spBar.fillAmount = _currSP / _data.SP;
        if (_currSP < 0) _currSP = 0;
    }
    public void EnterBattle()
    {
        gameObject.SetActive(true);
        _spriteAnimator.Play("EnterBattle");
    }
}
