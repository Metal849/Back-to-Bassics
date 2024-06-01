using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerTraversalPawn>())
        {
            OnTrigger.Invoke();
        }
    }
}
