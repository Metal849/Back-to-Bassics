using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    public bool deflectionWindow { get; private set; }
    public bool dodging { get; set; }
    protected override void Awake()
    {
        base.Awake();
        _activeAttackRequesters = new Queue<IAttackRequester>();
        _traversalPawn = GetComponent<PlayerTraversalPawn>();
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
        DodgeDirection = DirectionHelper.GetVectorDirection(direction);
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
        if (IsDead || attacking) return;
        AnimatorStateInfo animatorState = _pawnSprite.Animator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("idle")) return;
        _pawnSprite.FaceDirection(new Vector3(direction.x, 0, 1));
        _pawnAnimator.Play($"Slash{DirectionHelper.GetVectorDirection(direction)}");
        _slashEffect.Play();
        // Set the Slash Direction
        SlashDirection = direction;
        SlashDirection.Normalize();
        //UIManager.Instance.PlayerSlash(SlashDirection);
        StartCoroutine(Attacking());
        //if (_activeAttackRequesters.Count > 0)
        //{
        //    // (Suggestion) Maybe you should process all requests?
        //    // Note we are dequeing!
        //    //_activeAttackRequesters.Peek().OnRequestDeflect(this);
        //}
        //else 
        if (_activeAttackRequesters.Count <= 0)
        {
            BattleManager.Instance.Enemy.Damage(_weaponData.Dmg);
            //BattleManager.Instance.Enemy.Lurch(_weaponData.Lrch); -> Uncomment this if we should do this?
            // BattleManager.Instance.Enemy.ApplyStatusAilments(_weaponData.ailments); -> uncomment you have defined this

            // (Past Ryan) Whatever the fuck I call completing/processing an attack as opposed to "receving a request" bullshit
            // (Current Ryan) Oh there it is lmao
            BattleManager.Instance.Enemy.ReceiveAttackRequest(this);
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
    public void ReceiveAttackRequest(IAttackRequester requester)
    {
        _activeAttackRequesters.Enqueue(requester);
        if (deflectionWindow)
        {
            requester.OnRequestDeflect(this);
            // TODO: Right here you can allow the player to attack right away if needed
        }
        //else if (blocking)
        //{
        //    // This is old and dying, kill me soon!
        //    requester.OnRequestBlock(this);
        //}
        else if (dodging)
        {
            requester.OnRequestDodge(this);
        }
         
    }

    public void CompleteAttackRequest(IAttackRequester requester)
    {
        if (_activeAttackRequesters.Peek() != requester)
        {
            Debug.LogError("Attack Request and Completion missmatch, expected attack requester \"" + _activeAttackRequesters.Peek() + "\" instead got \"" + requester + ".\"");
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
        _pawnSprite.Animator.Play("attack_blocked");
    }

    public void OnRequestDodge(IAttackReceiver receiver)
    {
        throw new System.NotImplementedException();
    }
    private IEnumerator Attacking()
    {
        //if (attacking && BattleManager.Instance.Enemy.ESM.IsOnState<EnemyStateMachine.Attacking>()) Lurch(2f);
        //StopAllCoroutines();
        // Divides duration beats into four sections!
        // First Divsion is early reveive
        // second divsion is deflection window
        // Third Division is late receive
        float divisionTime = _weaponData.AttackDuration / 4f;
        attacking = true;
        //Debug.Log("Punishment");
        yield return new WaitForSeconds(divisionTime /* * Conductor.quarter * Conductor.Instance.spb*/);
        deflectionWindow = true;
        //Debug.Log("Deflecting");
        yield return new WaitForSeconds(2 * divisionTime /* * Conductor.quarter * Conductor.Instance.spb*/);
        deflectionWindow = false;
        //Debug.Log("Punishment");
        yield return new WaitForSeconds(divisionTime /* * Conductor.quarter * Conductor.Instance.spb*/);
        attacking = false;
        //Debug.Log("Ready to slash");
    }
    //protected override void OnStagger()
    //{
    //    base.OnStagger();
    //    Unblock();
    //    _particleSystem.Play();
    //}
    //protected override void OnUnstagger()
    //{
    //    base.OnUnstagger();
    //    _particleSystem.Stop();
    //}
}