using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FireProjectileAction : EnemyAction
{
    [Header("Fire Projectile Action")]
    [SerializeField, Range(1, 100)] private int projectilePoolAmount;
    [SerializeField] private string animationName;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="proj">Reference to Projectile to Fire</param>
    /// <param name="speed">In Beats</param>
    /// <param name="position"> Relative to player</param>
    public void FireProjectileAtPlayer(FireProjectileNode node)
    {
        GameObject objRef = Pooler.Instance.Pool(node.projRef);
        if (objRef == null)
        {
            Pooler.Instance.PoolGameObject(node.projRef, projectilePoolAmount);
            objRef = Pooler.Instance.Pool(node.projRef);
        }
        Projectile proj = objRef.GetComponent<Projectile>();
        proj.transform.position = BattleManager.Instance.Player.playerCollider.position + node.relativeSpawnPosition;
        proj.Fire((BattleManager.Instance.Player.playerCollider.position - proj.transform.position) / (node.speed * Conductor.Instance.spb));
        
        // This Only talors to bassics, not in general
        parentPawn.SpriteAnimator.SetFloat("xdir", -node.relativeSpawnPosition.x);
        parentPawn.SpriteAnimator.Play("animationName");  
    }
}

[SerializeField]
public struct FireProjectileNode
{
    public GameObject projRef;
    [Tooltip("In Beats")] public float speed;
    public Vector3 relativeSpawnPosition;
}
