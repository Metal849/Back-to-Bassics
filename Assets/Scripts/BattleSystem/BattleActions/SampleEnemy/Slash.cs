using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : BattleAction, IAttackRequester
{
    [Header("Slash Action")]
    [SerializeField] private float _attackWindow = 0.5f;
    [SerializeField] private int _dmg;
    [SerializeField] private int _lrch;
    private PlayerBattlePawn _hitPlayerPawn;
    private float _attackTime;
    public override void StartAction()
    {
        base.StartAction();
        // Change this later
        Debug.Log("Slash Action");
        ParentPawn.SpriteAnimator.SetFloat("xdir", -1f);
        ParentPawn.SpriteAnimator.Play("slash_broadcast");
        // Then do attack slash
    }
    protected override void OnQuarterBeat()
    {
        if (_hitPlayerPawn == null || Conductor.Instance.Beat < _attackTime) return;
        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementMissTracker();
        //---------------------------------------
        ParentPawn.SpriteAnimator.ResetTrigger("performed");
        ParentPawn.SpriteAnimator.SetTrigger("performed");
        _hitPlayerPawn.Damage(_dmg);
        //_hitPlayerPawn.Lurch(_lrch); -> Should the player be punished SP as wewll?

        _hitPlayerPawn.CompleteAttackRequest(this);
        IsActive = false;
    }
    public void OnRequestBlock(IAttackReceiver receiver)
    {
        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementBlockTracker();
        //---------------------------------------
        ParentPawn.SpriteAnimator.ResetTrigger("blocked");
        ParentPawn.SpriteAnimator.SetTrigger("blocked");
        _hitPlayerPawn.Lurch(_lrch);
        _hitPlayerPawn = null;
        IsActive = false;
    }
    public void OnRequestDeflect(IAttackReceiver receiver)
    {
        // Require a specfic slash to process this
        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementParryTracker();
        //---------------------------------------
        ParentPawn.SpriteAnimator.ResetTrigger("deflected");
        ParentPawn.SpriteAnimator.SetTrigger("deflected");
        ParentPawn.Lurch(_hitPlayerPawn.WeaponData.Lrch);
        _hitPlayerPawn = null;
        IsActive = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        _hitPlayerPawn = other.GetComponent<PlayerBattlePawn>();
        if (_hitPlayerPawn == null) return;
        if (_hitPlayerPawn.blocking)
        {
            OnRequestBlock(_hitPlayerPawn);
            return;
        }
        _attackTime = Conductor.Instance.Beat + _attackWindow;
        _hitPlayerPawn.ReceiveAttackRequest(this);
    }
}
