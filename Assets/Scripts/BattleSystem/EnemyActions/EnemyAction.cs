using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Will be the inherited template for most battleactions!
/// </summary>
public abstract class EnemyAction : Conductable
{
    public EnemyBattlePawn ParentPawn { get; set; }
    public bool IsActive { get; protected set; }
    public virtual void StartAction()
    {
        IsActive = true;
    }
}
