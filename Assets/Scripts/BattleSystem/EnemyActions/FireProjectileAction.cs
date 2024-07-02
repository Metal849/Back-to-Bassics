using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FireProjectileAction : EnemyAction
{
    [SerializeField, Range(1, 100)] private int projectilePoolAmount;
    private void poolNewProjectile(GameObject projRef)
    {
        Pooler.Instance.PoolGameObject(projRef, projectilePoolAmount);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="proj">Reference to Projectile to Fire</param>
    /// <param name="speed">In Beats</param>
    /// <param name="position"> Relative to player</param>
    public void FireProjectileAtPlayer(GameObject objRef, float speed, Vector3 relativePosition)
    {
        Projectile proj = Pooler.Instance.Pool(objRef).GetComponent<Projectile>();
        proj.transform.position = BattleManager.Instance.Player.transform.position + relativePosition;
        proj.Fire((BattleManager.Instance.Player.transform.position - proj.transform.position) / (2 * Conductor.Instance.spb));
        // May need ancile methods to let animations be extendable here!
        parentPawn.SpriteAnimator.SetFloat("xdir", -relativePosition.x);
        parentPawn.SpriteAnimator.Play("music");  
    }
}

[SerializeField]
public struct FireProjectileNode
{
    public Projectile projRef;
}
