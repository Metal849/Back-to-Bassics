using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using TMPro;

public class PlayerBattlePawn : BattlePawn
{
    [Header("Player References")]
    [SerializeField] private DrawSpace _drawSpace;
    public bool blocking { get; private set; }
    public Direction CurrSlashDirection { get; private set; }
    public Vector2 SlashDirection { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        CurrSlashDirection = Direction.None;
        SlashDirection = Vector2.zero;
    }
    public void Block()
    {
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle") || blocking) return;
        blocking = true;
        _spriteAnimator.Play("Block");
        if (_activeAttackRequester != null)
        {
            _activeAttackRequester.OnReceiverBlock(this);
            _activeAttackRequester = null;
        }
    }
    public void Unblock()
    {  
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Block") || !blocking) return;
        blocking = false;
        _spriteAnimator.Play("Unblock");
    }
    public void Dodge(Direction direction)
    {
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle")) return;
        _spriteAnimator.Play("Dodge" + direction);
    }
    /// <summary>
    /// Slash with an amount of strength in a specified cardinal direction
    /// </summary>
    /// <param name="strength"></param>
    /// <param name="direction"></param>
    //public void Slash(float strength, Direction direction)
    //{
    //    AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
    //    if (!animatorState.IsName("Idle")) return;
    //    CurrSlashDirection = direction;
    //    if (_activeAttackRequester != null)
    //    {
    //        _activeAttackRequester.OnReceiverDeflect(this);
    //        _activeAttackRequester = null;
    //    }
        
    //    _slashText.text = CurrSlashDirection.ToString() + " slash at beat " + Conductor.Instance.Beat;
    //}
    /// <summary>
    /// Uses a raw normalized vector, wayyy cleaner!
    /// </summary>
    /// <param name="slashDirection"></param>
    public void Slash(Vector2 slashDirection)
    {
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle")) return;
        slashDirection.Normalize();
        if (_activeAttackRequester != null)
        {
            _activeAttackRequester.OnReceiverDeflect(this);
            _activeAttackRequester = null;
        }
    }
    private void Update()
    {
        if (IsDead) return;
        // Legacy Input System Memes
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
        if (Input.GetMouseButton(1))
        {
            Block();
        }
        if (Input.GetMouseButtonUp(1))
        {
            Unblock();
        }
    }
}
