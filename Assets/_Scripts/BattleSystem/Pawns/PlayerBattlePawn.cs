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
    [Header("Player Data")]
    [SerializeField] protected int comboMeterMax = 100;
    [SerializeField] protected int comboMeterCurr;
    [SerializeField] protected Combo[] combos;
    public int ComboMeterMax => comboMeterMax;
    public int ComboMeterCurr 
    { 
        get { return comboMeterCurr; } 
        set 
        { 
            comboMeterCurr = value;
            if (comboMeterCurr > comboMeterMax)
            {   
                comboMeterCurr = comboMeterMax;
            }
            if (comboMeterCurr < 0)
            {
                comboMeterCurr = 0;
            }
            UIManager.Instance.UpdateComboMeter(this);
        } 
    }
    private string comboString;
    private Dictionary<string, Combo> comboDict;
    private Coroutine comboStopper;
    private Coroutine attackingThread;
    protected override void Awake()
    {
        base.Awake();
        comboString = "";
        comboDict = new Dictionary<string, Combo>();
        foreach (Combo combo in combos)
        {
            comboDict.Add(combo.StrId, combo);
        }
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
        if (comboString.Length >= 4)
        {
            comboString = "";
        }
        if (slash)
        {
            if (SlashDirection == Vector2.left)
            {
                comboString += "W";
                UIManager.Instance.ComboDisplay.AddCombo("W");
            }
            else if (SlashDirection == Vector2.right)
            {
                comboString += "E";
                UIManager.Instance.ComboDisplay.AddCombo("E");
            }
            else if (SlashDirection == Vector2.up) 
            {
                comboString += "N";
                UIManager.Instance.ComboDisplay.AddCombo("N");
            }
            else if (SlashDirection == Vector2.down) 
            {
                comboString += "S";
                UIManager.Instance.ComboDisplay.AddCombo("S");
            }
        }
        else
        {
            switch(DodgeDirection)
            {
                case Direction.North:
                    comboString += "n";
                    UIManager.Instance.ComboDisplay.AddCombo("n");
                    break;
                case Direction.South:
                    comboString += "s";
                    UIManager.Instance.ComboDisplay.AddCombo("s");
                    break;
                case Direction.West:
                    comboString += "w";
                    UIManager.Instance.ComboDisplay.AddCombo("w");
                    break;
                case Direction.East:
                    comboString += "e";
                    UIManager.Instance.ComboDisplay.AddCombo("e");
                    break;
            }
        }
        if (comboStopper != null)
        {
            StopCoroutine(comboStopper);
        }
        comboStopper = StartCoroutine(TimeToResetCombo());
        if (!comboDict.ContainsKey(comboString)) return;
        if (ComboMeterCurr < comboDict[comboString].Cost)
        {
            UIManager.Instance.ComboDisplay.HideCombo();
            comboString = "";
            StopCoroutine(comboStopper);
            return;
        }
        UIManager.Instance.ComboDisplay.ValidCombo();
        ComboMeterCurr -= comboDict[comboString].Cost;
        comboDict[comboString].StartComboAttack();
        
    }
    private IEnumerator TimeToResetCombo()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ComboDisplay.HideCombo();
        comboString = "";
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
            ComboMeterCurr += 1;
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
        // First Divsion is early reveive
        // second divsion is deflection window
        // Third Division is late receive
        float divisionTime = _weaponData.AttackDuration / 4f;
        attacking = true;
        deflectionWindow = false;
        yield return new WaitForSeconds(divisionTime /* * Conductor.quarter * Conductor.Instance.spb*/);
        deflectionWindow = true;
        yield return new WaitForSeconds(2 * divisionTime /* * Conductor.quarter * Conductor.Instance.spb*/);
        deflectionWindow = false;
        // Direct Attack when no attack requesters
        // This is where combo strings should be processed
        if (!deflected && _activeAttackRequesters.Count <= 0)
        {
            // Process Combo Strings here if you have enough!
            if (BattleManager.Instance.Enemy.ReceiveAttackRequest(this))
            {
                BattleManager.Instance.Enemy.Damage(_weaponData.Dmg);
                updateCombo(true);
            }
            
            // BattleManager.Instance.Enemy.ApplyStatusAilments(_weaponData.ailments); -> uncomment when you have defined this

        }
        yield return new WaitForSeconds(divisionTime /* * Conductor.quarter * Conductor.Instance.spb*/);
        attacking = false;
        _pawnAnimator.Play($"SlashEnd");
        deflected = false;
    }
}