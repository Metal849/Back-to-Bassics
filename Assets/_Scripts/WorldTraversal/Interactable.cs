using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable Specifications")]
    [SerializeField] private bool requireInput;
    protected abstract void Interact();
    private void OnTriggerEnter(Collider other)
    {
        if (requireInput) return;
        if (other.GetComponent<PlayerTraversalPawn>())
        {
            Interact();
        }
    }
}
