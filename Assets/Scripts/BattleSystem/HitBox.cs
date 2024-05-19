using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour, IAttackRequester
{
    public void OnRequestBlock(IAttackReceiver receiver)
    {
        throw new System.NotImplementedException();
    }

    public void OnRequestDeflect(IAttackReceiver receiver)
    {
        throw new System.NotImplementedException();
    }
    private void OnTriggerEnter(Collider other)
    {
        var pawn = other.GetComponent<PlayerBattlePawn>();
        if (pawn == null) return;
        pawn.ReceiveAttackRequest(this);
    }
}
