using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwipe : BattleAction
{
    public override void Broadcast(Direction dir) 
    {
        if (dir != Direction.East && dir != Direction.West)
        {
            Debug.LogError("Slash Needs to be either East or West");
            return;
        }
        ParentPawn.SpriteAnimator.SetFloat("xdir", dir == Direction.East ? 1f : -1f);
        ParentPawn.SpriteAnimator.Play("slash_broadcast");
        Debug.Log(ParentPawn.Data.name + " broadcasts " + dir.ToString() + " Slash");
    }
    public override void Perform(Direction dir) 
    {
        if (dir != Direction.East && dir != Direction.West)
        {
            Debug.LogError("Slash Needs to be either East or West");
            return;
        }
        ParentPawn.SpriteAnimator.SetFloat("xdir", dir == Direction.East ? 1f : -1f);
        ParentPawn.SpriteAnimator.Play("slash_perform");
        Debug.Log(ParentPawn.Data.name + " performs " + dir.ToString() + " Slash");
    }
}
