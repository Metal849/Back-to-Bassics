using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePawn : Conductable
{
    [Header("References")]
    [SerializeField] protected BattlePawnData _data;
    [SerializeField] protected Animator _spriteAnimator;

    [Header("Battle Pawn Data")]
    [SerializeField] protected int _currHP;
    [SerializeField] protected int _currSP;

    public int HP => _currHP;
    public int SP => _currSP;
    public BattlePawnData Data => _data;
    public Animator SpriteAnimator => _spriteAnimator;

    protected virtual void Awake()
    {
        _currHP = _data.HP;
        _currSP = _data.SP;
        gameObject.SetActive(false);
    }
    public void Damage(int amount)
    {
        _currHP -= amount;
        if (_currHP < 0) _currHP = 0;
    }
    public void EnterBattle()
    {
        gameObject.SetActive(true);
        _spriteAnimator.Play("EnterBattle");
    }
}
