using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Conductable, IAttackRequester
{
    [Header("Projectile Specs")]
    [SerializeField] private int _dmg;
    private float _speed;
    private Rigidbody _rb;
    private Direction _opposeDirection;
    public bool isDestroyed { get; private set; }
    private PlayerBattlePawn _hitPlayerPawn;
    private float _attackWindow;
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
        _opposeDirection = DirectionHelper.GetVectorDirection(-velocity);
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
        _opposeDirection = DirectionHelper.GetVectorDirection(-_rb.velocity);
        isDestroyed = false;
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider collision)
    {
        _hitPlayerPawn = collision.GetComponentInParent<PlayerBattlePawn>();
        if (_hitPlayerPawn == null) return;
        if (_hitPlayerPawn.blocking)
        {
            OnReceiverBlock(_hitPlayerPawn);
            return;
        }
        _hitPlayerPawn.AttackRequest(this);
        _attackWindow = Conductor.Instance.Beat + 0.5f;
        _rb.velocity = Vector2.zero;
    }
    protected override void OnQuarterBeat()
    {
        // Conditionals are yucky, thus do on event calls nub!
        //if (_hitPlayerPawn == null || (_hitPlayerPawn.CurrSlashDirection != _opposeDirection && !_hitPlayerPawn.blocking && Conductor.Instance.Beat < _attackWindow)) return;
        if (_hitPlayerPawn == null || Conductor.Instance.Beat < _attackWindow) return;
        if (_hitPlayerPawn.CurrSlashDirection == _opposeDirection)
        {
            Debug.Log("Parried On Beat");
            UIManager.Instance.IncrementParryTracker();
        }
        else if (_hitPlayerPawn.blocking)
        {
            Debug.Log("Blocked on Beat");
            UIManager.Instance.IncrementBlockTracker();
            _hitPlayerPawn.Lurch(_dmg);
        }
        else
        {
            Debug.Log("Miss on Beat");
            UIManager.Instance.IncrementMissTracker();
            _hitPlayerPawn.Damage(_dmg);
        }

        Destroy();
    }
    public void OnReceiverDeflect(IAttackReceiver receiver)
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
    public void OnReceiverBlock(IAttackReceiver receiver)
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
