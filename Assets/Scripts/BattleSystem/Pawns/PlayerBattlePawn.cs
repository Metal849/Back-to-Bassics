using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattlePawn : BattlePawn, IAttackRequester, IAttackReceiver
{
    [Header("Player References")]
    [SerializeField] private PlayerWeaponData _weaponData;
    public bool blocking { get; private set; }
    public Vector2 SlashDirection { get; private set; }
    private Queue<IAttackRequester> _activeAttackRequesters;

    public float AttackDamage { get => _weaponData.Dmg; }
    public float AttackLurch { get => _weaponData.Lrch; }
    protected override void Awake()
    {
        base.Awake();
        _activeAttackRequesters = new Queue<IAttackRequester>();
        SlashDirection = Vector2.zero;
    }
    #region Player Actions
    /// <summary>
    /// Processes blocks to any active attack requests.
    /// </summary>
    public void Block()
    {
        if (IsStaggered || IsDead) return;
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle") || blocking) return;
        blocking = true;
        _spriteAnimator.Play("Block");
        if (_activeAttackRequesters.Count > 0)
        {
            // (Suggestion) Maybe you should process all requests?
            _activeAttackRequesters.Dequeue().OnRequestBlock(this);
        }
    }
    /// <summary>
    /// Should Follow Blocking, the animation and the input.
    /// Might not need the !blocking check
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
        if (IsStaggered || IsDead) return;
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle")) return;
        _spriteAnimator.Play("Dodge" + direction);
    }
    /// <summary>
    /// Slash in a given direction. 
    /// If there are active attack requests, deflect them. 
    /// Otherwise request an attack to the enemy pawn.
    /// </summary>
    /// <param name="slashDirection"></param>
    public void Slash(Vector2 slashDirection)
    {
        if (IsStaggered || IsDead) return;
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle")) return;
        slashDirection.Normalize();
        if (_activeAttackRequesters.Count > 0)
        {
            // (Suggestion) Maybe you should process all requests?
            _activeAttackRequesters.Dequeue().OnRequestDeflect(this);
        }
        else 
        {
            // Request Attack to EnemyBattlePawn
            BattleManager.Instance.Enemy.ReceiveAttackRequest(this);
        }
    }
    #endregion
    /// <summary>
    /// Player cannot recover sp while blocking -> Could be brought further upward, in case we have items that use this method...
    /// </summary>
    /// <param name="amount"></param>
    public override void RecoverSP(float amount)
    {
        // Technically inefficent due to second method call, but good for readablity and modularity!
        if (!blocking) base.RecoverSP(amount);
    }
    private void Update()
    {
        // (TEMP) Legacy Input System Memes
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
    #region IAttackReceiver Methods
    public void ReceiveAttackRequest(IAttackRequester requester)
    {
        _activeAttackRequesters.Enqueue(requester);
    }

    public void CompleteAttackRequest(IAttackRequester requester)
    {
        if (_activeAttackRequesters.Peek() != requester)
        {
            Debug.Log("Attack Request and Completion missmatch, expected attack requester \"" + _activeAttackRequesters.Peek() + "\" instead got \"" + requester + ".\"");
            return;
        }
        _activeAttackRequesters.Dequeue();
    }
    #endregion

    public void OnRequestDeflect(IAttackReceiver receiver)
    {
        throw new System.NotImplementedException();
    }

    public void OnRequestBlock(IAttackReceiver receiver)
    {
        throw new System.NotImplementedException();
    }
}
