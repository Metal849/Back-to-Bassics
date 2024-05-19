using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// By default, all status ailments are require 100 buildup to max out
/// </summary>
public abstract class StatusAilment : Conductable
{
    protected AilmentType type;
    protected BattlePawn _pawn;
    protected float _maxBuildUp;
    protected float _currBuildUp;
    protected float _recoveryTime;
    public bool Inflicted {  get; private set; }
    protected virtual void Awake()
    {
        _pawn = GetComponent<BattlePawn>();
        _currBuildUp = 0;
        _maxBuildUp = 100;
    }
    /// <summary>
    /// Build when inflicted, don't build up when already inflicted
    /// </summary>
    /// <param name="amount"></param>
    public virtual void BuildUp(int amount)
    {
        if (Inflicted) return;
        //_currBuildUp += amount * (1 - _pawn.Data.resistence[A]);
        if (_currBuildUp >= _maxBuildUp)
        {
            _currBuildUp = _maxBuildUp;
            Inflicted = true;
        }
    }
    protected override void OnFullBeat()
    {
        //_currBuildUp -= _pawn.Data.resistence[A] * 10;
    }
}
public enum AilmentType
{
    None = 0,

}
