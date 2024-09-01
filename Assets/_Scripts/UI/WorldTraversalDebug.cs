using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTraversalDebug : MonoBehaviour
{
    public void RotatePonchoYBy(float y)
    {
        GameManager.Instance.PC.TraversalPawn.AddToRotationOnYAxis(y);
    }
}
