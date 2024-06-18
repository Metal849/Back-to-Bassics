using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class TraversalPawn : MonoBehaviour
{
    [Header("Traversal Pawn Specs")]
    [SerializeField] private float speed;
    public bool movingToDestination { get; private set; }
    protected Vector3 destinationTarget;
    private void FixedUpdate()
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
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
    public void MoveToDestination(Vector3 destination)
    {
        movingToDestination = true;
        destinationTarget = new Vector3(destination.x, destination.y, destination.z);
    }
}
