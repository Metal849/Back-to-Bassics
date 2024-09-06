using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemyBattle : EnemyBattlePawn
{
    private void Start()
    {
        BattleManager.Instance.Player.ComboMeterCurr = BattleManager.Instance.Player.ComboMeterMax;
    }
    public override bool ReceiveAttackRequest(IAttackRequester requester)
    {
        PlayerBattlePawn player = requester as PlayerBattlePawn;
        if (player != null)
        {
            player.ComboMeterCurr = player.ComboMeterMax;
        }
        return true;
    }
}
