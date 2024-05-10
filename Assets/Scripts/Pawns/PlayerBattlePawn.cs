using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattlePawn : BattlePawn
{
    [Header("Player References")]
    [SerializeField] private DrawSpace _drawSpace;
    public void Block()
    {
        _spriteAnimator.Play("Block");
    }
    public void UnBlock()
    {
        _spriteAnimator.Play("UnBlock");
    }
    public void Dodge(Direction direction)
    {
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle")) return;
        switch (direction)
        {
            case Direction.West:
                _spriteAnimator.Play("DodgeWest");
                break;
            case Direction.East:
                _spriteAnimator.Play("DodgeEast");
                break;
            case Direction.South:
                _spriteAnimator.Play("DodgeSouth");
                break;
            default:
                Debug.LogError("Not Valid Dodge Direction Inputed");
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Dodge(Direction.West);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Dodge(Direction.East);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Dodge(Direction.South);
        }
    }
}
