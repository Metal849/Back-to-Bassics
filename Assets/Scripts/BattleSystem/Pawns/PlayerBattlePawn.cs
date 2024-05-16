using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattlePawn : BattlePawn
{
    [Header("Player References")]
    [SerializeField] private DrawSpace _drawSpace;
    public bool blocking { get; private set; }
    public Vector2 SlashDirection { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        SlashDirection = Vector2.zero;
    }
    #region Player Actions
    /// <summary>
    /// Block :3
    /// </summary>
    public void Block()
    {
        if (IsStaggered) return;
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle") || blocking) return;
        blocking = true;
        _spriteAnimator.Play("Block");
        if (_activeAttackRequester != null)
        {
            _activeAttackRequester.OnRequestBlock(this);
            _activeAttackRequester = null;
        }
    }
    /// <summary>
    /// Should Follow Blocking, the animation and the input. Might not need the !blocking check
    /// </summary>
    public void Unblock()
    {
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Block") || !blocking) return;
        blocking = false;
        _spriteAnimator.Play("Unblock");
    }
    public void Dodge(Direction direction)
    {
        if (IsStaggered) return;
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle")) return;
        _spriteAnimator.Play("Dodge" + direction);
    }
    /// <summary>
    /// Slash in a given direction, if an attack was requested deflect it, otherwise slash whatever is in front of player.
    /// </summary>
    /// <param name="slashDirection"></param>
    public void Slash(Vector2 slashDirection)
    {
        if (IsStaggered) return;
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle")) return;
        slashDirection.Normalize();
        if (_activeAttackRequester != null)
        {
            _activeAttackRequester.OnRequestDeflect(this);
            _activeAttackRequester = null;
        }
    }
    #endregion
    /// <summary>
    /// Player cannot recover sp while blocking -> Could be brought further upward, in case we have items that use this method...
    /// </summary>
    /// <param name="amount"></param>
    public override void RecoverSP(float amount)
    {
        // Technically inefficent due to second method call, but good for readable and modularity!
        if (!blocking) base.RecoverSP(amount);
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
