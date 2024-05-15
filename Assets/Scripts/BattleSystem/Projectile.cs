using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Conductable
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
        isDestroyed = true;
        gameObject.SetActive(false);
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
        _hitPlayerPawn.ReceiveAttackRequest();
        _attackWindow = Conductor.Instance.Beat + 0.5f;
        _rb.velocity = Vector2.zero;
    }
    protected override void OnQuarterBeat()
    {
        if (_hitPlayerPawn == null || (_hitPlayerPawn.CurrSlashDirection != _opposeDirection && Conductor.Instance.Beat < _attackWindow)) return;

        if (_hitPlayerPawn.CurrSlashDirection == _opposeDirection)
        {
            Debug.Log("Parried");
        }
        else
        {
            Debug.Log("Miss");
            _hitPlayerPawn.Damage(_dmg);
        }

        isDestroyed = true;
        _hitPlayerPawn = null;
        gameObject.SetActive(false);
    }
}
