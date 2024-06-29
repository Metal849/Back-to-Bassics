using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Will be the inherited template for most battleactions!
/// </summary>
public abstract class EnemyAction : Conductable
{
    [SerializeField] protected EnemyBattlePawn parentPawn;
    public bool IsActive { get; protected set; }
    private void Awake()
    {
        IsActive = false;
        parentPawn = GetComponentInParent<EnemyBattlePawn>();
        if (parentPawn == null) 
        {
            Debug.LogError($"Enemy Action \"{gameObject.name}\" could not find Enemy Pawn Parent");
            return;
        }
        parentPawn.AddEnemyAction(this);
        //Debug.Log($"Enemy Action \"{gameObject.name}\" is type: {GetType()}");
    }
    public virtual void StartAction()
    {
        IsActive = true;
        Enable();
    }
    public virtual void StopAction()
    {
        IsActive = false;
        Disable();
        parentPawn.OnActionComplete();
    }
}
