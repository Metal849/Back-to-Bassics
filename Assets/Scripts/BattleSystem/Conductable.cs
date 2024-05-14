using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour that using OnBeat() for updating logic as apposed to using Update()
/// </summary>
public abstract class Conductable : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Conductor.Instance.OnBeat += OnBeat;
    }

    /// <summary>
    /// Simlar to Unity Update but called on every beat fired by the Conductor.
    /// </summary>
    protected abstract void OnBeat();
}
