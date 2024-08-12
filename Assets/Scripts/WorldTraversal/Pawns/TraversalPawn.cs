using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class TraversalPawn : MonoBehaviour
{
    [Header("Traversal Pawn Specs")]
    [SerializeField] private float speed;
    [SerializeField] protected Animator _spriteAnimator;
    public Animator SpriteAnimator => _spriteAnimator;
    protected Animator _pawnAnimator;
    public bool movingToDestination { get; private set; }
    protected Vector3 destinationTarget;
    private Rigidbody _rb;
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _pawnAnimator = GetComponent<Animator>();
    }
    protected void FixedUpdate()
    {
        if (movingToDestination)
        {
            // TODO: This section right here is what is causing the floating of our character, see if you
            // Can change this where gravity is applied and the character isn't going to try to fly.
            // You might need to use the rigidbody component in order to manipulate their kinetic movement.
            transform.position = Vector3.MoveTowards(transform.position, destinationTarget, speed * Time.deltaTime);
            if (transform.position == destinationTarget) movingToDestination = false;
        }
    }

    // X is Right and Left, Y is Forward and Backward 
    public void Move(Vector3 direction)
    {
        direction.Normalize();
        _rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        Vector2 curr = new Vector2(_spriteAnimator.GetFloat("DirX"), _spriteAnimator.GetFloat("DirY"));
        //Animation Related :PPPPP
        if (direction.x != 0)
        {
            _spriteAnimator.SetFloat("DirX", Mathf.Sign(direction.x));
        }
        if (direction.z != 0)
        {
            _spriteAnimator.SetFloat("DirY", Mathf.Sign(direction.z));
        }
        Vector2 change = new Vector2(_spriteAnimator.GetFloat("DirX"), _spriteAnimator.GetFloat("DirY"));
        float angle = Vector2.SignedAngle(curr, change);
        if (angle > 0)
        {
            _spriteAnimator.SetTrigger("FlipCCW");
        }
        else if (angle < 0)
        {
            _spriteAnimator.SetTrigger("FlipCW");
        }
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
    public void MoveToDestination(Vector3 destination)
    {
        movingToDestination = true;
        destinationTarget = new Vector3(destination.x, destination.y, destination.z);
    }
}
