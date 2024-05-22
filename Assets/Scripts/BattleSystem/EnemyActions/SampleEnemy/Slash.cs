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
    private float _animationCompleteTime;
    private float _attackTime;
    private float _nextSequenceTime;
    private bool _slashing;
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
        ParentPawn.ESM.Transition<EnemyStateMachine.Idle>();
    }
    protected override void OnQuarterBeat()
    {
        //Debug.Log("NextSequenceTime: " + _nextSequenceTime + "\nAttackTime: " + _attackTime);
        if (!IsActive) return;

        // Slash Attack Window
        if (!_slashing && !_attackSequence[currIdx].performed && _attackSequence[currIdx].isSlash && Conductor.Instance.Beat >= _animationCompleteTime)
        {
            SlashAttackWindow();
        }

        // Peform Slash
        if (_slashing && Conductor.Instance.Beat >= _attackTime)
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
        
        ParentPawn.SpriteAnimator.SetTrigger("blocked");
        
        BattleManager.Instance.Player.Lurch(_attackSequence[currIdx].lrch);

        _slashing = false;
        _attackSequence[currIdx].performed = true;
        BattleManager.Instance.Player.CompleteAttackRequest(this);
    }
    public void OnRequestDeflect(IAttackReceiver receiver)
    {
        if (DirectionHelper.MaxAngleBetweenVectors(-_attackSequence[currIdx].direction, BattleManager.Instance.Player.SlashDirection, 5f)
            && Conductor.Instance.Beat < _attackTime)
        {
            // (TEMP) Manual DEBUG UI Tracker -------
            UIManager.Instance.IncrementParryTracker();
            //---------------------------------------
            
            ParentPawn.SpriteAnimator.SetTrigger("deflected");
            
            ParentPawn.Lurch(BattleManager.Instance.Player.WeaponData.Lrch);

            _slashing = false;
            _attackSequence[currIdx].performed = true;
            BattleManager.Instance.Player.CompleteAttackRequest(this);
        }      
    }
    private void PerformSlashOnPlayer()
    {
        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementMissTracker();
        //---------------------------------------
        
        ParentPawn.SpriteAnimator.SetTrigger("performed");
        
        BattleManager.Instance.Player.Damage(_attackSequence[currIdx].dmg);
        //_hitPlayerPawn.Lurch(_attackSequence[currIdx].lrch); -> Should the player be punished SP as wewll?

        _slashing = false;
        _attackSequence[currIdx].performed = true;
        BattleManager.Instance.Player.CompleteAttackRequest(this);
    }
    private void TraverseSequence()
    {     
        // Reset attack performance trigger
        if (currIdx >= 0 && currIdx < _attackSequence.Length)
        {
            _attackSequence[currIdx].performed = false;
        }
        // Traversal
        if (++currIdx >= _attackSequence.Length)
        {
            StopAction();
            return;
        }

        // Do we have an animation to play?
        string animation = "slash_" + _attackSequence[currIdx].animationName;
        bool hasStateAnimation = ParentPawn.SpriteAnimator.HasState(0, Animator.StringToHash(animation));
        
        // Timing
        float animationTime = 0;
        if (hasStateAnimation)
        {
            // Character Animation Direction
            ParentPawn.SpriteAnimator.SetFloat("xdir", _attackSequence[currIdx].direction.x);
            ParentPawn.SpriteAnimator.SetFloat("ydir", _attackSequence[currIdx].direction.y);

            // Play Animation
            if (_attackSequence[currIdx].isSlash)
            {
                ParentPawn.SpriteAnimator.ResetTrigger("performed");
                ParentPawn.SpriteAnimator.ResetTrigger("deflected");
                ParentPawn.SpriteAnimator.ResetTrigger("blocked");
            }
            ParentPawn.SpriteAnimator.Play(animation);

            // Character Speed Sync with Conductor per beat
            animationTime = ParentPawn.SpriteAnimator.GetCurrentAnimatorStateInfo(0).length;
            //int beats = Mathf.RoundToInt(animationTime / Conductor.Instance.spb);
            //ParentPawn.SpriteAnimator.SetFloat("speed", (beats * Conductor.Instance.spb) / animationTime);
            //animationTime = beats * Conductor.Instance.spb;

            // For Slashing time
            _animationCompleteTime = Conductor.Instance.Beat + animationTime;
        }
        
        // Next sequence time calculation
        _nextSequenceTime = Conductor.Instance.Beat 
            + (_attackSequence[currIdx].includeAnimatorTime ? animationTime : 0)
            + _attackSequence[currIdx].delayToNextAttack * 0.25f 
            + _attackSequence[currIdx].attackWindow * 0.25f;
        
    }
    private void SlashAttackWindow()
    {
        BattleManager.Instance.Player.ReceiveAttackRequest(this);
        _attackTime = Conductor.Instance.Beat + _attackSequence[currIdx].attackWindow * 0.25f;
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
        public bool isSlash;
        public bool isCharged;
        public bool includeAnimatorTime;
        public int dmg;
        public int lrch;
        public Vector2 direction;
        public string animationName;
        [Tooltip("In Quarter Beats")] public int attackWindow;
        [Tooltip("In Quarter Beats")] public int delayToNextAttack;

        // Dynamic Values
        [HideInInspector] public bool performed; 
    }
}
