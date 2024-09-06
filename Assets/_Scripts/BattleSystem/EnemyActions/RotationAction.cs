using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAction : EnemyAction
{
    [SerializeField] private GameObject knifeFab;
    [SerializeField] private Spinning[] spinners;
    protected override void OnStartAction()
    {
        foreach (Spinning spinner in spinners)
        {
            if (spinner != null) spinner.speed = spinner.minSpeed;
        }
        
    }
    protected override void OnStopAction()
    {
        foreach (Spinning spinner in spinners)
        {
            if (spinner != null) spinner.speed = 0f;
        }
    }
}
