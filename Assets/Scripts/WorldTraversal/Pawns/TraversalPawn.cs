using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public abstract class TraversalPawn : MonoBehaviour
{
    [Header("Traversal Pawn Specs")]
    [SerializeField] private float speed;
    [SerializeField] protected Animator _spriteAnimator;
    public Animator SpriteAnimator => _spriteAnimator;
    protected Animator _pawnAnimator;
    protected PawnSprite _pawnSprite;
    protected CharacterController _characterController;
    public bool movingToDestination { get; private set; }
    protected Vector3 destinationTarget;
    private Rigidbody _rb;
    private Quaternion pawnFaceRotation;
    protected virtual void Awake()
    {
        pawnFaceRotation = transform.rotation;
        _characterController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();
        _pawnAnimator = GetComponent<Animator>();
        _pawnSprite = GetComponentInChildren<PawnSprite>();
    }
    protected virtual void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, pawnFaceRotation, Time.fixedDeltaTime * 0.5f);
        if (movingToDestination)
        {
            // TODO: This section right here is what is causing the floating of our character, see if you
            // Can change this where gravity is applied and the character isn't going to try to fly.
            // You might need to use the rigidbody component in order to manipulate their kinetic movement.
            transform.position = Vector3.MoveTowards(transform.position, destinationTarget, speed * Time.fixedDeltaTime);
            if (transform.position == destinationTarget) movingToDestination = false;
        }
    }

    // X is Right and Left, Z is Forward and Backward 
    public virtual void Move(Vector3 direction)
    {
        direction.Normalize();
        Vector3 move = transform.rotation * direction * speed;
        _rb.velocity = new Vector3(move.x, _rb.velocity.y, move.z);
        //_rb.AddForce(move - _rb.velocity);
        //_rb.MovePosition(transform.position + move * Time.deltaTime);
        //_characterController.Move(transform.rotation * direction * speed * Time.deltaTime);
        _pawnSprite.Animator.SetBool("moving", direction != Vector3.zero);
        _pawnSprite.FaceDirection(direction);
    }
    public void MoveToDestination(Vector3 destination)
    {
        movingToDestination = true;
        destinationTarget = new Vector3(destination.x, destination.y, destination.z);
    }
    public void RotateOnYAxis(float y)
    {
        pawnFaceRotation = Quaternion.Euler(0, y, 0);
    }
}
