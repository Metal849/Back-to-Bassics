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
        Conductor.Instance.OnFirstBeat += OnFirstBeat;
        Conductor.Instance.OnBeat += OnBeat;
        Conductor.Instance.OnLastBeat += OnLastBeat;
    }

    protected virtual void OnFirstBeat() { OnBeat(); }
    /// <summary>
    /// Simlar to Unity Update but called on every beat fired by the Conductor.
    /// </summary>
    protected virtual void OnBeat() { }
    protected virtual void OnLastBeat() { OnBeat(); }
}
