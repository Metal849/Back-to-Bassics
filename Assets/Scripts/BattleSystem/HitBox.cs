using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour, IAttackRequester
{
    public void OnRequestDeflect(IAttackReceiver receiver)
    {
        //PlayerBattlePawn player = receiver as PlayerBattlePawn;
        //if (player == null) return;
        //// Did receiver deflect in correct direction?
        //if (!DirectionHelper.MaxAngleBetweenVectors(-_rb.velocity, player.SlashDirection, 5f)) return;

        //// (TEMP) Manual DEBUG UI Tracker -------
        //UIManager.Instance.IncrementParryTracker();
        ////---------------------------------------
        //AudioManager.Instance.PlayPlayerSlash(player.WeaponData.SlashHitSound, player.transform.position);
        //_hitPlayerPawn.CompleteAttackRequest(this);
        //Destroy();
    }
    public void OnRequestBlock(IAttackReceiver receiver)
    {
        //// (TEMP) Manual DEBUG UI Tracker -------
        //UIManager.Instance.IncrementBlockTracker();
        ////---------------------------------------
        ////_hitPlayerPawn.Lurch(_dmg);
        //_hitPlayerPawn.CompleteAttackRequest(this);
        //Destroy();
    }
    public void OnRequestDodge(IAttackReceiver receiver)
    {
        // Nothing Happens Here :o
    }

    private void OnTriggerEnter(Collider other)
    {
        var pawn = other.GetComponent<PlayerBattlePawn>();
        if (pawn == null) return;
        pawn.ReceiveAttackRequest(this);
    }
}
