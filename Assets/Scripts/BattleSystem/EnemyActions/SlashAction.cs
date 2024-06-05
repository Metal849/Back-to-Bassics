using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Assertions;

public class SlashAction : EnemyAction, IAttackRequester
{
    [Header("Slash Action")]
    [SerializeField] private SlashNote[] _attackSequence;
    private int currIdx;
    
    //private float _attackTime;
    
    private const string PERFORM = "slash_perform";
    private const string BROADCAST = "slash_broadcast";
    private const string SUCCESS = "slash_success";
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
            Debug.Log($"Hit Beat: {Conductor.Instance.Beat}");
            Debug.Log($"Hit Time: {Time.time}");
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

            AnimationClip clip = ParentPawn.SpriteAnimator.GetCurrentAnimatorClipInfo(0)[0].clip;
            AnimatorStateInfo info = ParentPawn.SpriteAnimator.GetCurrentAnimatorStateInfo(0);
            // Character Speed Sync with Conductor per beat
            int beats = Mathf.FloorToInt((5f / 60f) / Conductor.Instance.spb);
            if (beats == 0) beats = 1;
            float syncedAnimationTime = beats * Conductor.Instance.spb;
            //ParentPawn.SpriteAnimator.runtimeAnimatorController.animationClips
            // Set speed of animator to that of the animation time
            ParentPawn.SpriteAnimator.SetFloat("speed", (5f / 60f) / syncedAnimationTime);
            _animationCompleteTime = Conductor.Instance.Beat + beats;
            Debug.Log($"Beats: {beats}");
            Debug.Log($"Start Beat: {Conductor.Instance.Beat}, End Beat: {_animationCompleteTime}");
            Debug.Log($"Start Time: {Time.time}, End Time: {Time.time + syncedAnimationTime}");
            Debug.Log($"Current SPB: {Conductor.Instance.spb}");
            Debug.Log($"New Animation Duration: {syncedAnimationTime}");
            Debug.Log($"Original Animation Duration: {(5f / 60f)}");
            // Increase Sequence Transition Time
            // Should probably include The later animation states, so you probably should handle swapping to them in here
            // instead of the animator :L
            // Currently they rely on the attack end delay so its kind wack
            _nextSequenceTime += _attackSequence[currIdx].includeAnimatorTime ? beats : 0;
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
        // (TEMP) DEBUG UI Tracker -------
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
            // (TEMP) DEBUG UI Tracker -------
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
        // (TEMP) DEBUG UI Tracker -------
        UIManager.Instance.IncrementMissTracker();
        //---------------------------------------
        
        //ParentPawn.SpriteAnimator.SetTrigger("performed");
        ParentPawn.SpriteAnimator.Play(SUCCESS);
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
        // (Ryan) Commented until the following is done:
        //      If the broadcasts time is lower than the animation broadcast time,
        //      Then we sync of the animation time to the broadcast time.
        //      Otherwise just use the given broadcast time.

        //int beats = Mathf.RoundToInt(info.length / Conductor.Instance.spb);
        //ParentPawn.SpriteAnimator.SetFloat("speed", info.length / (beats * Conductor.Instance.spb));
        ParentPawn.SpriteAnimator.SetFloat("speed", 1f);
        //_animationCompleteTime = Conductor.Instance.Beat + beats * Conductor.Instance.spb;
        // For Broadcast time
        _animationCompleteTime = Conductor.Instance.Beat + _attackSequence[currIdx].broadcastTime * Conductor.quarter;
        

        // Next sequence time calculation
        _nextSequenceTime = Conductor.Instance.Beat
            + _attackSequence[currIdx].broadcastTime * Conductor.quarter
            + _attackSequence[currIdx].delayToNextAttack * Conductor.quarter;
    }
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
