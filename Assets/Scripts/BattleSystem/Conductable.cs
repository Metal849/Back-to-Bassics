using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour that uses Conductor beating behaviour
/// </summary>
public abstract class Conductable : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Conductor.Instance.OnFirstBeat += OnFirstBeat;
        Conductor.Instance.OnQuarterBeat += OnQuarterBeat;
        Conductor.Instance.OnHalfBeat += OnHalfBeat;
        Conductor.Instance.OnFullBeat += OnFullBeat;
        Conductor.Instance.OnLastBeat += OnLastBeat;
    }

    protected virtual void OnFirstBeat() { OnFullBeat(); }
    protected virtual void OnQuarterBeat() { }
    protected virtual void OnHalfBeat() { }
    protected virtual void OnFullBeat() { }
    protected virtual void OnLastBeat() { OnFullBeat(); }
}
