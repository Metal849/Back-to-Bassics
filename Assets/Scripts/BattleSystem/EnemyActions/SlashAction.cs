using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

/// <summary>
/// DOES NOT HANDLE SEQUENCES ANYMORE!
/// </summary>
public class SlashAction : EnemyAction, IAttackRequester
{
    //[Header("Slash Action")]
    // SHOULD NOT HANDLE THIS ANYMORE!!!
    //[SerializeField] private SlashBeat[] _attackSequence;
    //private int currIdx;

    //private float _attackTime;

    #region technical
    //private float _animationCompleteTime;
    //private float _nextSequenceTime;
    //private bool _broadcasting;
    //private bool _performed;
    private bool _slashing;
    private SlashNode _currNode;

    #endregion

    //public void DynamicAnimatoClipUpdate()
    //{
    //    foreach (AnimationClip clip in parentPawn.SpriteAnimator.runtimeAnimatorController.animationClips) 
    //    {
    //        switch (clip.name)
    //        {
    //            case PERFORM:
    //                perform_time = clip.length;
    //                break;
    //            case BROADCAST:
    //                broadcast_time = clip.length;
    //                break;
    //            case SUCCESS:
    //                success_time = clip.length;
    //                break;

    //        }
    //    }
    //}
    public override void StartAction()
    {
        base.StartAction();
        //// Initial Broadcast of Slash
        //currIdx = -1;
        //TraverseSequence();
    }
    public void Slash(SlashNode node)
    {
        StartCoroutine(SlashThread(node));
    }
    private IEnumerator SlashThread(SlashNode node)
    {
        // Slash Initialization
        _currNode = node;
        parentPawn.SpriteAnimator.SetFloat("xdir", node.slashDirection.x);
        parentPawn.SpriteAnimator.SetFloat("ydir", node.slashDirection.y);
        // Animation Before Hit
        int beats = Mathf.RoundToInt(node.preHitClip.length / Conductor.Instance.spb);
        if (beats == 0) beats = 1;
        float syncedAnimationTime = beats * Conductor.Instance.spb;
        parentPawn.SpriteAnimator.SetFloat("speed", node.preHitClip.length / syncedAnimationTime);
        parentPawn.SpriteAnimator.Play(node.preHitClip.name);

        yield return new WaitForSeconds(syncedAnimationTime);

        // Hit Moment
        _slashing = true;
        BattleManager.Instance.Player.ReceiveAttackRequest(this);
        if (_slashing)
        {
            PerformSlashOnPlayer();
            yield return new WaitForSeconds(node.postHitClip.length);
        }
        else
        {
            yield return new WaitForSeconds(node.postHitClip.length);
        }
    }
    protected override void OnQuarterBeat()
    {
        ////Debug.Log("NextSequenceTime: " + _nextSequenceTime + "\nAttackTime: " + _attackTime);
        //if (!IsActive) return;

        //// Slash Attack Window
        //if (!_performed && !_broadcasting && Conductor.Instance.Beat >= _animationCompleteTime)
        //{
        //    Debug.Log($"Hit Beat: {Conductor.Instance.Beat}");
        //    Debug.Log($"Hit Time: {Time.time}");
        //    _performed = true;
        //    _slashing = true;
        //    BattleManager.Instance.Player.ReceiveAttackRequest(this);
        //    if (_slashing) PerformSlashOnPlayer();
        //}
        //// THIS SHOULD BE THE ONLY PURPOSE OF ENEMY ACTIONS!
        //// Perform Slash Attack
        //if (_broadcasting && Conductor.Instance.Beat >= _animationCompleteTime)
        //{
        //    _broadcasting = false;
        //    parentPawn.SpriteAnimator.Play(PERFORM);
        //    //AnimatorStateInfo info = parentPawn.SpriteAnimator.GetCurrentAnimatorStateInfo(0);
        //    // Character Speed Sync with Conductor per beat
        //    int beats = Mathf.RoundToInt(perform_time / Conductor.Instance.spb);
        //    if (beats == 0) beats = 1;
        //    float syncedAnimationTime = beats * Conductor.Instance.spb;
        //    //parentPawn.SpriteAnimator.runtimeAnimatorController.animationClips
        //    // Set speed of animator to that of the animation time
        //    parentPawn.SpriteAnimator.SetFloat("speed", perform_time / syncedAnimationTime);
        //    _animationCompleteTime = Conductor.Instance.Beat + beats;
        //    Debug.Log($"Beats: {beats}");
        //    Debug.Log($"Start Beat: {Conductor.Instance.Beat}, End Beat: {_animationCompleteTime}");
        //    Debug.Log($"Start Time: {Time.time}, End Time: {Time.time + syncedAnimationTime}");
        //    Debug.Log($"Current SPB: {Conductor.Instance.spb}");
        //    Debug.Log($"New Animation Duration: {syncedAnimationTime}");
        //    Debug.Log($"Original Animation Duration: {perform_time}");
        //    // Increase Sequence Transition Time
        //    // Should probably include The later animation states, so you probably should handle swapping to them in here
        //    // instead of the animator :L
        //    // Currently they rely on the attack end delay so its kind wack
        //    _nextSequenceTime += beats;
        //}

        //// Next Sequence
        //if (Conductor.Instance.Beat >= _nextSequenceTime)
        //{
        //    TraverseSequence();
        //}

    }
    #region IAttackRequester Methods
    public void OnRequestBlock(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player == null) return;
        // (TEMP) DEBUG UI Tracker -------
        UIManager.Instance.IncrementBlockTracker();
        //---------------------------------------

        //parentPawn.SpriteAnimator.SetTrigger("blocked");
        parentPawn.SpriteAnimator.Play(_currNode.deflectedClip.name);
        //player.Lurch(_attackSequence[currIdx].lrch);

        _slashing = false;
        player.CompleteAttackRequest(this);
    }
    public void OnRequestDeflect(IAttackReceiver receiver) 
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player != null && DirectionHelper.MaxAngleBetweenVectors(-_currNode.slashDirection, player.SlashDirection, 5f))
        {
            // (TEMP) DEBUG UI Tracker -------
            UIManager.Instance.IncrementParryTracker();
            //---------------------------------------

            //parentPawn.SpriteAnimator.SetTrigger("deflected");
            parentPawn.SpriteAnimator.Play(_currNode.deflectedClip.name);
            //parentPawn.Lurch(BattleManager.Instance.Player.WeaponData.Lrch);

            _slashing = false;
            player.CompleteAttackRequest(this);
        }      
    }
    public void OnRequestDodge(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player != null && _currNode.dodgeDirections.Contains(player.DodgeDirection)) 
        {
            //parentPawn.SpriteAnimator.SetTrigger("performed");
            parentPawn.SpriteAnimator.Play(_currNode.postHitClip.name);
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

        //parentPawn.SpriteAnimator.SetTrigger("performed");
        //parentPawn.SpriteAnimator.Play(SUCCESS);
        parentPawn.SpriteAnimator.Play(_currNode.postHitClip.name);
        BattleManager.Instance.Player.Damage(_currNode.dmg);
        //_hitPlayerPawn.Lurch(_attackSequence[currIdx].lrch); -> Should the player be punished SP as wewll?

        _slashing = false;
        BattleManager.Instance.Player.CompleteAttackRequest(this);
    }
    private void TraverseSequence()
    {
        // Reset attack performance trigger
        //_performed = false;
        //// Traversal
        //if (++currIdx >= _attackSequence.Length)
        //{
        //    StopAction();
        //    return;
        //}

        //// Timing
        //// Character Animation Direction
        //parentPawn.SpriteAnimator.SetFloat("xdir", _attackSequence[currIdx].direction.x);
        //parentPawn.SpriteAnimator.SetFloat("ydir", _attackSequence[currIdx].direction.y);

        //// Broadcast Attack Animation
        //parentPawn.SpriteAnimator.ResetTrigger("performed");
        //parentPawn.SpriteAnimator.ResetTrigger("deflected");
        //parentPawn.SpriteAnimator.ResetTrigger("blocked");
        //parentPawn.SpriteAnimator.Play(BROADCAST);
        //_broadcasting = true;

        //// Character Speed Sync with Conductor per beat
        //// (Ryan) Commented until the following is done:
        ////      If the broadcasts time is lower than the animation broadcast time,
        ////      Then we sync of the animation time to the broadcast time.
        ////      Otherwise just use the given broadcast time.

        ////int beats = Mathf.RoundToInt(info.length / Conductor.Instance.spb);
        ////ParentPawn.SpriteAnimator.SetFloat("speed", info.length / (beats * Conductor.Instance.spb));
        //parentPawn.SpriteAnimator.SetFloat("speed", 1f);
        ////_animationCompleteTime = Conductor.Instance.Beat + beats * Conductor.Instance.spb;
        //// For Broadcast time
        //_animationCompleteTime = Conductor.Instance.Beat + _attackSequence[currIdx].broadcastTime * Conductor.quarter;
        

        //// Next sequence time calculation
        //_nextSequenceTime = Conductor.Instance.Beat
        //    + _attackSequence[currIdx].broadcastTime * Conductor.quarter
        //    + _attackSequence[currIdx].delayToNextAttack * Conductor.quarter;
    }
}

[Serializable]
public struct SlashNode
{
    public AnimationClip preHitClip;
    public AnimationClip postHitClip;
    public AnimationClip deflectedClip;

    public bool isCharged;
    public int dmg;
    public Vector2 slashDirection;
    public Direction[] dodgeDirections;
}
