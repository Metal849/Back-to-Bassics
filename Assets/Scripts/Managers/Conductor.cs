using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Conductor : Singleton<Conductor>
{
    public float Beat { get; private set; }
    public float spb {  get; private set; }
    private bool beating;
    
    public event Action OnQuarterBeat;
    public event Action OnHalfBeat;
    public event Action OnFullBeat;
    public event Action OnFirstBeat;
    public event Action OnLastBeat;
    public void Awake()
    {
        InitializeSingleton();
    }
    public void BeginConducting(int bpm)
    {
        if (beating) 
        {
            Debug.LogWarning("Conductor was issued to conduct when it already is conducting");
            return;
        }
        Beat = 0;
        spb = 60f / bpm;
        beating = true;
        OnFirstBeat.Invoke();
        StartCoroutine(Conduct());
    }
    public void StopConducting()
    {
        if (!beating)
        {
            Debug.LogWarning("Conductor was issued to stop conducting when it already is not conducting");
            return;
        }
        beating = false;
    } 

    private IEnumerator Conduct()
    {
        float quarterTime = spb / 4f;
        OnFirstBeat.Invoke();
        while (beating)
        {
            yield return new WaitForSeconds(quarterTime);
            Beat += 0.25f;
            OnQuarterBeat?.Invoke();
            
            yield return new WaitForSeconds(quarterTime);
            Beat += 0.25f;
            OnQuarterBeat?.Invoke();
            OnHalfBeat?.Invoke();
            
            yield return new WaitForSeconds(quarterTime);
            Beat += 0.25f;
            OnQuarterBeat?.Invoke();
            
            yield return new WaitForSeconds(quarterTime);
            Beat += 0.25f;
            OnQuarterBeat?.Invoke();
            OnHalfBeat?.Invoke();
            OnFullBeat?.Invoke();
            
        }
        OnLastBeat.Invoke();
    }
}
