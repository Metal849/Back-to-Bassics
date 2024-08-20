using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAction : EnemyAction
{
    [SerializeField] private GameObject knifeFab;
    [SerializeField] private Spinning spinner;
    public override void StartAction()
    {
        base.StartAction();
        spinner.speed = 8f;
    }
    public override void StopAction()
    {
        base.StopAction();
        spinner.speed = 0f;
    }
}
