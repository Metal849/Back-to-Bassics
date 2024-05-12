using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Will be the inherited template for most battleactions!
/// </summary>
public abstract class BattleAction : MonoBehaviour
{
    public BattlePawn ParentPawn { get; set; }
    public abstract void Broadcast(Direction dir);
    public abstract void Perform(Direction dir);
}
