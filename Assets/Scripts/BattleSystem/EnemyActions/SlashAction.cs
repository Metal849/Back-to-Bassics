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
    // technical
    private readonly string[] directions_seperator = new string[] { "_east", "_west", "_north", "_south" };
    private bool _slashing;
    private SlashNode _currNode;

    public void Slash(SlashNode node)
    {
        Debug.Log("Slash Called");
        StartCoroutine(SlashThread(node));
    }
    private IEnumerator SlashThread(SlashNode node)
    {
        // Slash Initialization
        _currNode = node;
        parentPawn.SpriteAnimator.SetFloat("xdir", node.slashDirection.x);
        parentPawn.SpriteAnimator.SetFloat("ydir", node.slashDirection.y);
        // Animation Before Hit
        //int beats = Mathf.RoundToInt(node.preHitClip.length / Conductor.Instance.spb);
        //if (beats == 0) beats = 1;
        //float syncedAnimationTime = beats * Conductor.Instance.spb;
        //parentPawn.SpriteAnimator.SetFloat("speed", node.preHitClip.length / syncedAnimationTime);
        parentPawn.SpriteAnimator.Play("slash_perform");
        yield return new WaitUntil(() => parentPawn.SpriteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f);
        Debug.Log("Performed");
        // Hit Moment
        _slashing = true;
        BattleManager.Instance.Player.ReceiveAttackRequest(this);
        if (_slashing)
        {
            PerformSlashOnPlayer();
        }
        yield return new WaitUntil(() => parentPawn.SpriteAnimator.GetCurrentAnimatorStateInfo(0).IsName("slash_success") ||
        parentPawn.SpriteAnimator.GetCurrentAnimatorStateInfo(0).IsName("slash_deflected"));
        yield return new WaitUntil(() => parentPawn.SpriteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f);
        Debug.Log("Done");
        //parentPawn.SpriteAnimator.Play("idle");
    }
    public void OnRequestBlock(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player == null) return;
        // (TEMP) DEBUG UI Tracker -------
        UIManager.Instance.IncrementBlockTracker();
        //---------------------------------------

        parentPawn.SpriteAnimator.SetTrigger("blocked");
        //parentPawn.SpriteAnimator.Play(_currNode.deflectedClip.name);
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

            parentPawn.SpriteAnimator.SetTrigger("deflected");
            //parentPawn.SpriteAnimator.Play("slash_deflected");
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
            parentPawn.SpriteAnimator.SetTrigger("performed");
            //parentPawn.SpriteAnimator.Play(_currNode.postHitClip.name);
            _slashing = false;
            player.CompleteAttackRequest(this);
        }

    }
    private void PerformSlashOnPlayer()
    {
        // (TEMP) DEBUG UI Tracker -------
        UIManager.Instance.IncrementMissTracker();
        //---------------------------------------

        parentPawn.SpriteAnimator.SetTrigger("performed");
        //parentPawn.SpriteAnimator.Play("slash_success");
        BattleManager.Instance.Player.Damage(_currNode.dmg);
        //_hitPlayerPawn.Lurch(_attackSequence[currIdx].lrch); -> Should the player be punished SP as wewll?

        _slashing = false;
        BattleManager.Instance.Player.CompleteAttackRequest(this);
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
