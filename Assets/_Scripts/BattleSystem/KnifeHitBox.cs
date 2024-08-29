using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHitBox : MonoBehaviour, IAttackRequester
{
    [SerializeField] private int damage;
    [field: SerializeField] public int health { get; private set; }
    [SerializeField] private Spinning spinner;
    private float resetSpeed = 0f;
    public bool OnRequestDeflect(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;

        // Did player deflect in correct direction?
        if (player == null 
            || !DirectionHelper.MaxAngleBetweenVectors(spinner.ccw ? Vector2.left : Vector2.right, player.SlashDirection, 5f)) 
            return false;

        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementParryTracker();
        //---------------------------------------
        health -= 1;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            spinner.speed += resetSpeed;
            spinner.ChangeDirection(resetSpeed);
        }
        resetSpeed += 0.2f;
        // (TEMP)----------- This is dumb IK---------------------
        BattleManager.Instance.Enemy.Damage(1);
        //-------------------------------------------------------
        player.CompleteAttackRequest(this);
        return true;
    }
    public bool OnRequestBlock(IAttackReceiver receiver)
    {
        //// (TEMP) Manual DEBUG UI Tracker -------
        //UIManager.Instance.IncrementBlockTracker();
        ////---------------------------------------
        ////_hitPlayerPawn.Lurch(_dmg);
        //_hitPlayerPawn.CompleteAttackRequest(this);
        //Destroy();
        return true;
    }
    public bool OnRequestDodge(IAttackReceiver receiver)
    {
        // Nothing Happens Here :o
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerBattlePawn pawn))
        {
            if (pawn.ReceiveAttackRequest(this))
            {
                pawn.Damage(damage);
                pawn.CompleteAttackRequest(this);
            }
        }
    }
}
