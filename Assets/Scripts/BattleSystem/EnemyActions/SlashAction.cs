using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

/// <summary>
/// DOES NOT HANDLE SEQUENCES ANYMORE!
/// </summary>
public class SlashAction : EnemyAction, IAttackRequester
{
    [Header("Slash Action")]
    [SerializeField] private string animationName;
    [SerializeField] private AnimationClip preHitClip;
    [SerializeField] private AnimationClip broadcastClip;
    public float preHitClipLengthInBeats => preHitClip.length / parentPawn.EnemyData.SPB;
    public float broadcastClipLengthInBeats => broadcastClip.length / parentPawn.EnemyData.SPB;
    // technical
    private bool _slashing;
    private SlashNode _currNode;
    public void Broadcast(Direction direction)
    {
        Vector2 slashDirection = DirectionHelper.GetVectorFromDirection(direction);
        // **************Revise this***********
        // The y value here is facing forward
        parentPawnSprite.FaceDirection(new Vector3(-slashDirection.x, 0, -1));
        parentPawnSprite.Animator.SetFloat("x_slashDir", slashDirection.x);
        parentPawnSprite.Animator.SetFloat("y_slashDir", slashDirection.y);
        parentPawnSprite.Animator.Play($"{animationName}_broadcast");
    }
    public void Slash(SlashNode node)
    {
        StartCoroutine(SlashThread(node));
    }
    private IEnumerator SlashThread(SlashNode node)
    {
        // Slash Initialization
        _currNode = node;

        // Animation Before Hit -> Setup animation speed
        //int beats = Mathf.RoundToInt(node.preHitClip.length / Conductor.Instance.spb);
        //if (beats == 0) beats = 1;
        //float syncedAnimationTime = beats * Conductor.Instance.spb;
        //parentPawn.SpriteAnimator.SetFloat("speed", node.preHitClip.length / syncedAnimationTime);
        // Direction Setup
        // The y value here is facing forward
        parentPawnSprite.FaceDirection(new Vector3(-_currNode.slashVector.x, 0, -1));
        parentPawnSprite.Animator.SetFloat("x_slashDir", _currNode.slashVector.x);
        parentPawnSprite.Animator.SetFloat("y_slashDir", _currNode.slashVector.y);

        parentPawnSprite.Animator.Play($"{animationName}_perform");
        yield return new WaitUntil(() => parentPawnSprite.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f);
        // Hit Moment
        _slashing = true;
        BattleManager.Instance.Player.ReceiveAttackRequest(this);
        if (_slashing)
        {
            PerformSlashOnPlayer();
        }
        yield return new WaitUntil(() => parentPawnSprite.Animator.GetCurrentAnimatorStateInfo(0).IsName($"{animationName}_success") ||
        parentPawnSprite.Animator.GetCurrentAnimatorStateInfo(0).IsName($"{animationName}_deflected"));
        yield return new WaitUntil(() => parentPawnSprite.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f);
    }
    public void OnRequestBlock(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player == null) return;
        // (TEMP) DEBUG UI Tracker -------
        UIManager.Instance.IncrementBlockTracker();
        //---------------------------------------

        parentPawnSprite.Animator.SetTrigger("blocked");

        _slashing = false;
        player.CompleteAttackRequest(this);
    }
    public void OnRequestDeflect(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player != null && DirectionHelper.MaxAngleBetweenVectors(-_currNode.slashVector, player.SlashDirection, 5f))
        {
            // (TEMP) DEBUG UI Tracker -------
            UIManager.Instance.IncrementParryTracker();
            //---------------------------------------

            parentPawnSprite.Animator.SetTrigger("deflected");
            AudioManager.Instance.PlayPlayerSlash(player.WeaponData.SlashHitSound, player.transform.position);
            if (_currNode.staggersParent)
            {
                parentPawn.Stagger();
            }

            _slashing = false;
            player.CompleteAttackRequest(this);
        }
    }
    public void OnRequestDodge(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player != null && _currNode.dodgeDirections.Contains(player.DodgeDirection))
        {
            parentPawnSprite.Animator.SetTrigger("performed");

            _slashing = false;
            player.CompleteAttackRequest(this);
        }

    }
    private void PerformSlashOnPlayer()
    {
        // (TEMP) DEBUG UI Tracker -------
        UIManager.Instance.IncrementMissTracker();
        //---------------------------------------

        parentPawnSprite.Animator.SetTrigger("performed");
        BattleManager.Instance.Player.Damage(_currNode.dmg);

        _slashing = false;
        BattleManager.Instance.Player.CompleteAttackRequest(this);
    }
}

[Serializable]
public struct SlashNode
{
    public Direction slashDirection;
    public bool isCharged;
    public bool staggersParent;
    public int dmg;
    public Direction[] dodgeDirections;
    public Vector2 slashVector => DirectionHelper.GetVectorFromDirection(slashDirection);
}
