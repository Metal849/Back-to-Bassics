using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class TraversalPawn : MonoBehaviour
{
    [Header("Traversal Pawn Specs")]
    [SerializeField] private float speed;
    protected bool movingToDestination;
    protected Vector3 destinationTarget;
    private void Update()
    {
        if (movingToDestination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationTarget, speed * Time.deltaTime);
            if (transform.position == destinationTarget) movingToDestination = false;
        }
    }

    // X is Right and Left, Y is Forward and Backward 
    public void Move(Vector2 direction)
    {
        direction.Normalize();
        Vector3 displacement = new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z + direction.y);
        transform.position = Vector3.MoveTowards(transform.position, displacement, speed * Time.deltaTime);
    }
    public void MoveToDestination(Vector2 destination)
    {
        movingToDestination = true;
        destinationTarget = new Vector3(destination.x, transform.position.y, destination.y);
    }
}
