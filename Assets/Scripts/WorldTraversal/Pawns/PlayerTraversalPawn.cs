using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraversalPawn : TraversalPawn
{
    public void Slash(Vector2 slashDirection)
    {
        Debug.Log($"Traversal Slash {DirectionHelper.GetVectorDirection(slashDirection)}");
    }
}
