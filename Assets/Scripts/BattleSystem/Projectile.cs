using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Conductable
{
    [Header("Projectile Specs")]
    [SerializeField] private int _dmg;
    [SerializeField] private int _speedInBeats;
    private float _speed;
    private Rigidbody _rb;
    private Direction _opposeDirection;
    public bool isDestroyed { get; private set; }
    #region Unity Messages
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        isDestroyed = true;
        gameObject.SetActive(false);
    }
    #endregion
    protected override void OnFirstBeat()
    {
        _speed = 1 / (_speedInBeats * Conductor.Instance.spb);
    }
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
        var pawn = collision.GetComponentInParent<PlayerBattlePawn>();
        if (pawn == null) return;
        if (pawn.CurrSlashDirection == _opposeDirection)
        {
            Debug.Log("Parried");
        }
        else
        {
            Debug.Log("Miss");
            pawn.Damage(_dmg);
        }

        isDestroyed = true;
        gameObject.SetActive(false);
    }
}
