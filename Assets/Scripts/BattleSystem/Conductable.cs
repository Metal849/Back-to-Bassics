using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour that has access to OnBeat() for updating beating logic
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
    protected virtual void OnBeat() { }
}
