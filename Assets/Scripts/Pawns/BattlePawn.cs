using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePawn : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected BattlePawnData _data;
    [SerializeField] protected Animator _spriteAnimator;
    protected int _currHP;
    protected int _currSP;

    public int HP => _currHP;
    public int SP => _currSP;

    protected void Awake()
    {
        _currHP = _data.HP;
        _currSP = _data.SP;
    }
    public void Damage(int amount)
    {
        _currHP -= amount;
        if (_currHP < 0) _currHP = 0;
    }
}
