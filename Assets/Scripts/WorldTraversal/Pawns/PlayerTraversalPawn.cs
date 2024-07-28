using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerTraversalPawn : TraversalPawn
{
    [SerializeField] private VisualEffect _slashEffect;
    public void Slash(Vector2 slashDirection)
    {
        _pawnAnimator.Play($"Slash{DirectionHelper.GetVectorDirection(slashDirection)}");
        _slashEffect.Play();
        Debug.Log($"Traversal Slash {DirectionHelper.GetVectorDirection(slashDirection)}");
    }
}
