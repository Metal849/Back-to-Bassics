using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHitBox : MonoBehaviour, IAttackRequester
{
    [SerializeField] private int damage;
    [field: SerializeField] public int health { get; private set; }
    [SerializeField] private Spinning spinner;
    private float resetSpeed = 0f;
    public void OnRequestDeflect(IAttackReceiver receiver)
    {
        PlayerBattlePawn player = receiver as PlayerBattlePawn;
        if (player == null) return;
        //// Did receiver deflect in correct direction?
        if (!DirectionHelper.MaxAngleBetweenVectors(spinner.ccw ? Vector2.left : Vector2.right, player.SlashDirection, 5f)) return;

        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementParryTracker();
        //---------------------------------------
        health -= 1;
        spinner.ChangeDirectionRandomSpeed();
        resetSpeed += 0.2f;
        player.CompleteAttackRequest(this);
        // (TEMP)----------- This is dumb IK---------------------
        BattleManager.Instance.Enemy.Damage(1);
        //-------------------------------------------------------
        if (health <= 0) Destroy(gameObject);   
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
