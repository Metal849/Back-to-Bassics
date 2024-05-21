using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slash : EnemyAction, IAttackRequester
{
    [Header("Slash Action")]
    [SerializeField] private SlashNote[] _attackSequence;
    private int currIdx;
    private float _attackTime;
    private float _nextSequenceTime;
    public override void StartAction()
    {
        base.StartAction();
        // Initial Broadcast of Slash
        currIdx = -1;
        TraverseSequence();
    }
    public override void StopAction()
    {
        base.StopAction();
        ParentPawn.SpriteAnimator.Play("Idle");
    }
    protected override void OnQuarterBeat()
    {
        if (!IsActive) return;

        // Attack Window
        if (_attackSequence[currIdx].isSlash && Conductor.Instance.Beat >= _attackTime && !_attackSequence[currIdx].performed)
        {
            PerformSlashOnPlayer();
        }

        // Next Sequence
        if (Conductor.Instance.Beat >= _nextSequenceTime)
        {
            TraverseSequence();
        }

    }
    public void OnRequestBlock(IAttackReceiver receiver)
    {
        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementBlockTracker();
        //---------------------------------------
        ParentPawn.SpriteAnimator.ResetTrigger("blocked");
        ParentPawn.SpriteAnimator.SetTrigger("blocked");
        BattleManager.Instance.Player.Lurch(_attackSequence[currIdx].lrch);
        _attackSequence[currIdx].performed = true;
        BattleManager.Instance.Player.CompleteAttackRequest(this);
    }
    public void OnRequestDeflect(IAttackReceiver receiver)
    {
        if (DirectionHelper.MaxAngleBetweenVectors(_attackSequence[currIdx].direction, BattleManager.Instance.Player.SlashDirection, 5f)
            && Conductor.Instance.Beat >= _attackTime)
        {
            // (TEMP) Manual DEBUG UI Tracker -------
            UIManager.Instance.IncrementParryTracker();
            //---------------------------------------
            ParentPawn.SpriteAnimator.ResetTrigger("deflected");
            ParentPawn.SpriteAnimator.SetTrigger("deflected");
            ParentPawn.Lurch(BattleManager.Instance.Player.WeaponData.Lrch);
            _attackSequence[currIdx].performed = true;
            BattleManager.Instance.Player.CompleteAttackRequest(this);
        }
        else
        {
            PerformSlashOnPlayer();
        }
        
    }
    private void PerformSlashOnPlayer()
    {
        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementMissTracker();
        //---------------------------------------
        ParentPawn.SpriteAnimator.ResetTrigger("performed");
        ParentPawn.SpriteAnimator.SetTrigger("performed");
        BattleManager.Instance.Player.Damage(_attackSequence[currIdx].dmg);
        //_hitPlayerPawn.Lurch(_attackSequence[currIdx].lrch); -> Should the player be punished SP as wewll?

        BattleManager.Instance.Player.CompleteAttackRequest(this);
        _attackSequence[currIdx].performed = true;
    }
    private void TraverseSequence()
    {
        if (++currIdx >= _attackSequence.Length)
        {
            StopAction();
            return;
        }
        // Animation
        ParentPawn.SpriteAnimator.SetFloat("xdir", _attackSequence[currIdx].direction.x);
        ParentPawn.SpriteAnimator.SetFloat("ydir", _attackSequence[currIdx].direction.y);
        ParentPawn.SpriteAnimator.Play("slash_" + _attackSequence[currIdx].animationName);

        // Timeing
        if (_attackSequence[currIdx].isSlash)
        {
            _attackTime = Conductor.Instance.Beat + _attackSequence[currIdx].attackWindow * 0.25f;
            BattleManager.Instance.Player.ReceiveAttackRequest(this);
        }
        
        _nextSequenceTime = Conductor.Instance.Beat + _attackSequence[currIdx].delayToNextAttack * 0.25f + _attackSequence[currIdx].attackWindow * 0.25f;
        
    }
    // Avoiding Collider Implementation
    //private void OnTriggerEnter(Collider other)
    //{
    //    _hitPlayerPawn = other.GetComponent<PlayerBattlePawn>();
    //    if (_hitPlayerPawn == null) return;
    //    if (_hitPlayerPawn.blocking)
    //    {
    //        OnRequestBlock(_hitPlayerPawn);
    //        return;
    //    }
    //    _attackTime = Conductor.Instance.Beat + _attackWindow;
    //    _hitPlayerPawn.ReceiveAttackRequest(this);
    //}
    [Serializable]
    public struct SlashNote
    {
        public bool isSlash;
        public bool isCharged;
        public int dmg;
        public int lrch;
        public Vector2 direction;
        public string animationName;
        [Tooltip("In Quarter Beats")] public int attackWindow;
        [Tooltip("In Quarter Beats")] public int delayToNextAttack;
        [HideInInspector] public bool performed; 
    }
}
