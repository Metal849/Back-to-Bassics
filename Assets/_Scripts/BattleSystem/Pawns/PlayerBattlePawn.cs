using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static EnemyStateMachine;

/// <summary>
/// Playable Battle Pawn
/// </summary>
[RequireComponent(typeof(PlayerController), typeof(PlayerTraversalPawn))]
public class PlayerBattlePawn : BattlePawn, IAttackRequester, IAttackReceiver
{
    [Header("Player References")]
    [SerializeField] private PlayerWeaponData _weaponData;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private VisualEffect _slashEffect;
    public Transform playerCollider;
    private PlayerTraversalPawn _traversalPawn;
    public PlayerWeaponData WeaponData => _weaponData;
    public Vector2 SlashDirection { get; private set; }
    public Direction DodgeDirection { get; private set; }
    private Queue<IAttackRequester> _activeAttackRequesters;
    public float AttackDamage { get => _weaponData.Dmg; }
    public bool attacking { get; private set; }
    private bool deflected;
    public bool deflectionWindow { get; private set; }
    public bool dodging { get; set; }
    private ComboManager _comboManager;
    private Coroutine attackingThread;
    protected override void Awake()
    {
        base.Awake();
        _activeAttackRequesters = new Queue<IAttackRequester>();
        _traversalPawn = GetComponent<PlayerTraversalPawn>();
        _comboManager = GetComponent<ComboManager>();
        SlashDirection = Vector2.zero;
    }
    // This will start a battle
    public void EngageEnemy(EnemyBattlePawn enemy)
    {
        BattleManager.Instance.StartBattle(new EnemyBattlePawn[] { enemy });
    }
    #region Player Actions
    /// <summary>
    /// Processes blocks to any active attack requests.
    /// </summary>
    //public void Block()
    //{
    //    if (IsStaggered || IsDead) return;
    //    AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
    //    if (!animatorState.IsName("idle") || blocking) return;
    //    blocking = true;
    //    _spriteAnimator.Play("block");
    //    if (_activeAttackRequesters.Count > 0)
    //    {
    //        // (Suggestion) Maybe you should process all requests?
    //        // Note we are dequeing!
    //        _activeAttackRequesters.Peek().OnRequestBlock(this);
    //    }
    //}
    /// <summary>
    /// Should Follow Blocking, the animation and the input.
    /// Might not need the !blocking check
    /// </summary>
    //public void Unblock()
    //{
    //    AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
    //    if (!animatorState.IsName("block") || !blocking) return;
    //    blocking = false;
    //    _spriteAnimator.Play("unblock");
    //}
    public void Dodge(Vector2 direction)
    {
        if (IsDead) return;
        AnimatorStateInfo animatorState = _pawnSprite.Animator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("idle")) return;
        // (Past Ryan 1) Figure out a way to make the dodging false later
        // (Past Ryan 2) I'm sorry future ryan, but I have figured it out through very scuffed means
        // Check a file called OnDodgeEnd.cs
        // (Ryan) This really sucky
        // Merge to one state called Open
        DodgeDirection = DirectionHelper.GetVectorDirection(direction);
        updateCombo(false);
        StartCoroutine(DodgeThread(DodgeDirection.ToString().ToLower()));
    }
    private IEnumerator DodgeThread(string directionAnimation)
    {
        dodging = true;
        _pawnSprite.Animator.Play("dodge_" + directionAnimation);
        yield return new WaitUntil(() => _pawnSprite.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f 
        && _pawnSprite.Animator.GetCurrentAnimatorStateInfo(0).IsName("idle"));
        dodging = false;
    }
    /// <summary>
    /// Slash in a given direction. 
    /// If there are active attack requests, deflect them. 
    /// Otherwise request an attack to the enemy pawn.
    /// </summary>
    /// <param name="slashDirection"></param>
    public void Slash(Vector2 direction)
    {
        if (IsDead) return;
        //AnimatorStateInfo animatorState = _pawnSprite.Animator.GetCurrentAnimatorStateInfo(0);
        //if (!animatorState.IsName("idle")) return;
        _pawnSprite.FaceDirection(new Vector3(direction.x, 0, 1));
        _pawnAnimator.Play($"Slash{DirectionHelper.GetVectorDirection(direction)}");
        _slashEffect.Play();
        AudioManager.Instance.PlayOnShotSound(WeaponData.slashAirSound, transform.position);
        // Set the Slash Direction
        SlashDirection = direction;
        SlashDirection.Normalize();
        //UIManager.Instance.PlayerSlash(SlashDirection);
        if (attackingThread != null) StopCoroutine(attackingThread);
        attackingThread = StartCoroutine(Attacking());

        //if (_activeAttackRequesters.Count > 0)
        //{
        //    // (Suggestion) Maybe you should process all requests?
        //    // Note we are dequeing!
        //    //_activeAttackRequesters.Peek().OnRequestDeflect(this);
        //}
        //else   
    }
    private void updateCombo(bool slash)
    {
        if (slash)
        {
            if (SlashDirection == Vector2.left)
            {
                _comboManager.AppendToCombo('W');
            }
            else if (SlashDirection == Vector2.right)
            {
                _comboManager.AppendToCombo('E');
            }
            else if (SlashDirection == Vector2.up) 
            {
                _comboManager.AppendToCombo('N');
            }
            else if (SlashDirection == Vector2.down) 
            {
                _comboManager.AppendToCombo('S');
            }
        }
        else
        {
            switch(DodgeDirection)
            {
                case Direction.North:
                    _comboManager.AppendToCombo('n');
                    break;
                case Direction.South:
                    _comboManager.AppendToCombo('s');
                    break;
                case Direction.West:
                    _comboManager.AppendToCombo('w');
                    break;
                case Direction.East:
                    _comboManager.AppendToCombo('e');
                    break;
            }
        }
    }
    #endregion
    /// <summary>
    /// Player cannot recover sp while blocking -> Could be brought further upward, in case we have items that use this method...
    /// </summary>
    /// <param name="amount"></param>
    //public override void RecoverSP(float amount)
    //{
    //    // Technically inefficent due to second method call, but good for readablity and modularity!
    //    // o7 sp
    //    if (!blocking && !attacking) base.RecoverSP(amount);
    //}
    #region IAttackReceiver Methods
    public bool ReceiveAttackRequest(IAttackRequester requester)
    {
        _activeAttackRequesters.Enqueue(requester);
        if (/*!deflected && */ deflectionWindow && requester.OnRequestDeflect(this))
        {
            deflected = true;
            AudioManager.Instance.PlayOnShotSound(WeaponData.slashHitSound, transform.position);
            _comboManager.CurrComboMeterAmount += 1;
            return false;
        }
        if (dodging && requester.OnRequestDodge(this))
        {
            return false;
        }
        return true;
    }

    public void CompleteAttackRequest(IAttackRequester requester)
    {
        if (_activeAttackRequesters.Peek() != requester)
        {
            Debug.LogError($"Attack Request and Completion missmatch, expected attack requester \"{_activeAttackRequesters.Peek()}\" instead got \"{requester}.\"");
            return;
        }
        _activeAttackRequesters.Dequeue();
    }
    #endregion
    #region IAttackRequester Methods
    public bool OnRequestDeflect(IAttackReceiver receiver)
    {
        return true;
    }
    public bool OnRequestBlock(IAttackReceiver receiver)
    {
        _pawnSprite.Animator.Play("attack_blocked");
        return true;
    }

    public bool OnRequestDodge(IAttackReceiver receiver)
    {
        return true;
    }
    #endregion
    private IEnumerator Attacking()
    {
        //if (attacking && BattleManager.Instance.Enemy.ESM.IsOnState<EnemyStateMachine.Attacking>()) Lurch(2f);
        //StopAllCoroutines();
        // Divides duration beats into four sections!
        // First Divsion is early receive
        // second divsion is deflection window
        // Third Division is late receive
        if (BattleManager.Instance.Enemy.ReceiveAttackRequest(this))
        {
            BattleManager.Instance.Enemy.Damage(_weaponData.Dmg);

            updateCombo(true);

            BattleManager.Instance.Enemy.CompleteAttackRequest(this);
        }
        float divisionTime = _weaponData.AttackDuration / 4f;
        attacking = true;
        deflectionWindow = true;
        yield return new WaitForSeconds(divisionTime /* * Conductor.quarter * Conductor.Instance.spb*/);
        deflectionWindow = true;
        yield return new WaitForSeconds(2 * divisionTime /* * Conductor.quarter * Conductor.Instance.spb*/);
        deflectionWindow = true;
        // Direct Attack when no attack requesters
        // This is where combo strings should be processed
        if (!deflected && _activeAttackRequesters.Count <= 0)
        {
            // Process Combo Strings here if you have enough!
            //if (BattleManager.Instance.Enemy.ReceiveAttackRequest(this))
            //{
                //BattleManager.Instance.Enemy.Damage(_weaponData.Dmg);
                // Uncomment below when Status Ailments have been defined
                // BattleManager.Instance.Enemy.ApplyStatusAilments(_weaponData.ailments);

                //updateCombo(true);

                //BattleManager.Instance.Enemy.CompleteAttackRequest(this);
            //}
        }
        yield return new WaitForSeconds(divisionTime /* * Conductor.quarter * Conductor.Instance.spb*/);
        attacking = false;
        deflectionWindow = false;
        _pawnAnimator.Play($"SlashEnd");
        deflected = false;
    }
}