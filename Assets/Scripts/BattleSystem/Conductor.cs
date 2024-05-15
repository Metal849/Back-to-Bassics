using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Conductor : Singleton<Conductor>
{
    public int Beat { get; private set; }
    public float spb {  get; private set; }
    private bool beating;
    public event Action OnBeat;
    public event Action OnFirstBeat;
    public event Action OnLastBeat;
    public void Awake()
    {
        InitializeSingleton();
    }
    public void BeginBeating(int bpm)
    {
        if (beating) 
        {
            Debug.LogWarning("Conductor was issued to beat when it already is beating");
            return;
        }
        Beat = 0;
        spb = 60f / bpm;
        beating = true;
        OnFirstBeat.Invoke();
        StartCoroutine(Sequencing());
    }
    public void StopBeating()
    {
        if (!beating)
        {
            Debug.LogWarning("Conductor was issued to stop beating when it already is not beating");
            return;
        }
        beating = false;
        StopAllCoroutines();
        OnLastBeat.Invoke();
    } 

    private IEnumerator Sequencing()
    {
        while (true)
        {
            Beat++;
            OnBeat?.Invoke();
            yield return new WaitForSeconds(spb);      
        }
    }
}
