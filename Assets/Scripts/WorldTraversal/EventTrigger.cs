using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool destroyOnEnter;
    [Header("Events")]
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerTraversalPawn>())
        {
            onEnter.Invoke();
            if (destroyOnEnter) Destroy(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerTraversalPawn>())
        {
            onExit.Invoke();
        }
    }
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(EventTrigger))]
//public class EventTriggerEditor : Editor
//{
//    private readonly string[] directions = { "North", "South", "East", "West" };
//    public override void OnInspectorGUI()
//    {
//        EventTrigger et = target as EventTrigger;
//    }
//}
//#endif
