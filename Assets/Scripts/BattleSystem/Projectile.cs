using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Conductable, IAttackRequester
{
    [Header("Projectile Specs")]
    [SerializeField] private int _dmg;
    private float _speed;
    private Rigidbody _rb;
    public bool isDestroyed { get; private set; }
    private PlayerBattlePawn _hitPlayerPawn;
    private float _attackWindow;
    public float AttackDamage => _dmg;
    public float AttackLurch => _dmg;
    #region Unity Messages
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy();
    }
    #endregion
    /// <summary>
    /// Spawn a projectile with a particular speed
    /// </summary>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    public void Spawn(Vector3 position, Vector2 velocity)
    {
        transform.position = position;
        _rb.velocity = velocity;
        isDestroyed = false;
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Spawn Projectile based on conductor's rule speed
    /// </summary>
    /// <param name="position"></param>
    /// <param name="dir"></param>
    public void Spawn(Vector3 position, Direction dir)
    {
        transform.position = position;
        _rb.velocity = _speed * DirectionHelper.GetVectorFromDirection(dir);

        // Inefficent as heck, but does the job
        isDestroyed = false;
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider collision)
    {
        _hitPlayerPawn = collision.GetComponentInParent<PlayerBattlePawn>();
        if (_hitPlayerPawn == null) return;
        // TODO: Think of a better way for this to work without projectile needing to know player pawn's state
        // HINT: Use CompleteAttackRequest
        if (_hitPlayerPawn.blocking)
        {
            OnRequestBlock(_hitPlayerPawn);
            return;
        }
        _hitPlayerPawn.ReceiveAttackRequest(this);
        _attackWindow = Conductor.Instance.Beat + 0.5f;
        _rb.velocity = Vector2.zero;
    }
    protected override void OnQuarterBeat()
    {
        // TODO: Think of a better way for this to work without projectile needing to know player pawn's state
        // HINT: Use CompleteAttackRequest
        if (_hitPlayerPawn == null || Conductor.Instance.Beat < _attackWindow) return;

        // (TEMP) Manual DEBUG UI Tracker -------
        UIManager.Instance.IncrementMissTracker();
        //---------------------------------------

        _hitPlayerPawn.Damage(AttackDamage);

        _hitPlayerPawn.CompleteAttackRequest(this);
        Destroy();
    }
    public void OnRequestDeflect(IAttackReceiver receiver)
    {
        if (IsSlashWithinOpposeDirection(-_rb.velocity, _hitPlayerPawn.SlashDirection) && Conductor.Instance.Beat < _attackWindow)
        {
            UIManager.Instance.IncrementParryTracker();
        }
        else
        {
            UIManager.Instance.IncrementMissTracker();
            _hitPlayerPawn.Damage(_dmg);
        }
        Destroy();
    }
    public void OnRequestBlock(IAttackReceiver receiver)
    {
        UIManager.Instance.IncrementBlockTracker();
        _hitPlayerPawn.Lurch(_dmg);
        Destroy();
    }
    public bool IsSlashWithinOpposeDirection(Vector2 opposeDirection, Vector2 slashDirection)
    {
        return Vector2.Angle(slashDirection, opposeDirection) <= 5f;
    }
    public void Destroy()
    {
        isDestroyed = true;
        _hitPlayerPawn = null;
        gameObject.SetActive(false);
    }
}
