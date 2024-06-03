using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraversalPawn : TraversalPawn
{
    [Header("PlyaerTraversalPawn Specs")]
    [SerializeField] private float battlePositionOffest = -1.8f;
    public void Slash(Vector2 slashDirection)
    {
        Debug.Log($"Traversal Slash {DirectionHelper.GetVectorDirection(slashDirection)}");
    }
}
