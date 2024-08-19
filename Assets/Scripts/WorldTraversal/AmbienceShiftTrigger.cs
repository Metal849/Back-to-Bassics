using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmbienceShiftTrigger : MonoBehaviour
{
    [SerializeField] private string parameterName;
    [SerializeField] private float parameterValue;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerTraversalPawn>())
        {
            AudioManager.Instance.SetAmbienceParameter("beach", parameterName, parameterValue);
        }
    }
}
