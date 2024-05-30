using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class Slash : EnemyAction, IAttackRequester
{
    [Header("Slash Action")]
    [SerializeField] private SlashNote[] _attackSequence;
    private int currIdx;
    
    //private float _attackTime;
    
    private const string PERFORM = "slash_perform";
    private const string BROADCAST = "slash_broadcast";
    #region technical
    private float _animationCompleteTime;
    private float _nextSequenceTime;
    private bool _slashing;
    private bool _broadcasting;
    private bool _performed;
    #endregion
    public override void StartAction()
    {
        base.StartAction();
        // Initial Broadcast of Slash
        currIdx = -1;
        TraverseSequence();
    }
    protected override void OnQuarterBeat()
    {
        //Debug.Log("NextSequenceTime: " + _nextSequenceTime + "\nAttackTime: " + _attackTime);
        if (!IsActive) return;

        // Slash Attack Window
        if (!_performed && !_broadcasting && Conductor.Instance.Beat >= _animationCompleteTime)
        {
            _performed = true;
            _slashing = true;
            BattleManager.Instance.Player.ReceiveAttackRequest(this);
            if (_slashing) PerformSlashOnPlayer();
        }

        // Perform Slash Attack
        if (_broadcasting && Conductor.Instance.Beat >= _animationCompleteTime)
        {
            _broadcasting = false;
            ParentPawn.SpriteAnimator.Play(PERFORM);

            AnimatorStateInfo info = ParentPawn.SpriteAnimator.GetCurrentAnimatorStateInfo(0);
            _animationCompleteTime = Conductor.Instance.Beat + (info.length / Conductor.Instance.spb);
            // Character Speed Sync with Conductor per beat
            //int beats = Mathf.RoundToInt(animationTime / Conductor.Instance.spb);
            //ParentPawn.SpriteAnimator.SetFloat("speed", (beats * Conductor.Instance.spb) / animationTime);
            //animationTime = beats * Conductor.Instance.spb;

            // Increase Sequence Transition Time
            // Should probably include The later animation states, so you probably should handle swapping to them in here
            // instead of the animator :L
            _nextSequenceTime += _attackSequence[currIdx].includeAnimatorTime ? (info.length / Conductor.Instance.spb) : 0;
        }

        // Next Sequence
        if (Conductor.Instance.Beat >= _nextSequenceTime)
        {
            TraverseSequence();
        }

    }
    #region IAttackRequester Methods
    public void OnRequestBlock(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player == null) return;
        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementBlockTracker();
        //---------------------------------------
        
        ParentPawn.SpriteAnimator.SetTrigger("blocked");

        player.Lurch(_attackSequence[currIdx].lrch);

        _slashing = false;
        player.CompleteAttackRequest(this);
    }
    public void OnRequestDeflect(IAttackReceiver receiver) 
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player != null && DirectionHelper.MaxAngleBetweenVectors(-_attackSequence[currIdx].direction, player.SlashDirection, 5f))
        {
            // (TEMP) Manual DEBUG UI Tracker -------
            UIManager.Instance.IncrementParryTracker();
            //---------------------------------------
            
            ParentPawn.SpriteAnimator.SetTrigger("deflected");
            
            ParentPawn.Lurch(BattleManager.Instance.Player.WeaponData.Lrch);

            _slashing = false;
            player.CompleteAttackRequest(this);
        }      
    }
    public void OnRequestDodge(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player != null && _attackSequence[currIdx].dodgeDirections.Contains(player.DodgeDirection)) 
        {
            ParentPawn.SpriteAnimator.SetTrigger("performed");

            _slashing = false;
            player.CompleteAttackRequest(this);
        }
        
    }
    #endregion
    private void PerformSlashOnPlayer()
    {
        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementMissTracker();
        //---------------------------------------
        
        ParentPawn.SpriteAnimator.SetTrigger("performed");
        
        BattleManager.Instance.Player.Damage(_attackSequence[currIdx].dmg);
        //_hitPlayerPawn.Lurch(_attackSequence[currIdx].lrch); -> Should the player be punished SP as wewll?

        _slashing = false;
        BattleManager.Instance.Player.CompleteAttackRequest(this);
    }
    private void TraverseSequence()
    {
        // Reset attack performance trigger
        _performed = false;
        // Traversal
        if (++currIdx >= _attackSequence.Length)
        {
            StopAction();
            return;
        }

        // Timing
        // Character Animation Direction
        ParentPawn.SpriteAnimator.SetFloat("xdir", _attackSequence[currIdx].direction.x);
        ParentPawn.SpriteAnimator.SetFloat("ydir", _attackSequence[currIdx].direction.y);

        // Broadcast Attack Animation
        ParentPawn.SpriteAnimator.ResetTrigger("performed");
        ParentPawn.SpriteAnimator.ResetTrigger("deflected");
        ParentPawn.SpriteAnimator.ResetTrigger("blocked");
        ParentPawn.SpriteAnimator.Play(BROADCAST);
        _broadcasting = true;

        // Character Speed Sync with Conductor per beat
        //int beats = Mathf.RoundToInt(animationTime / Conductor.Instance.spb);
        //ParentPawn.SpriteAnimator.SetFloat("speed", (beats * Conductor.Instance.spb) / animationTime);
        //animationTime = beats * Conductor.Instance.spb;

        // For Broadcast time
        _animationCompleteTime = Conductor.Instance.Beat + _attackSequence[currIdx].broadcastTime * Conductor.quarter;
        

        // Next sequence time calculation
        _nextSequenceTime = Conductor.Instance.Beat
            + _attackSequence[currIdx].broadcastTime * Conductor.quarter
            + _attackSequence[currIdx].delayToNextAttack * Conductor.quarter;
    }
    // This old and deprecated
    private void SlashAttackWindow()
    {
        BattleManager.Instance.Player.ReceiveAttackRequest(this);
        //_attackTime = Conductor.Instance.Beat + _attackSequence[currIdx].attackWindow * Conductor.quarter;
        _slashing = true;
        if (BattleManager.Instance.Player.blocking)
        {
            OnRequestBlock(BattleManager.Instance.Player);
            return;
        }
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
        public bool isCharged;
        public bool includeAnimatorTime;
        public int dmg;
        public int lrch;
        public Vector2 direction;
        public Direction[] dodgeDirections;
        [Tooltip("In Quarter Beats")] public int broadcastTime;
        [Tooltip("In Quarter Beats")] public int delayToNextAttack;

        // Dynamic Values
        [HideInInspector] public bool performed; 
    }
}
