using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Conductor : Singleton<Conductor>
{
    private int _currBeat = 0;
    private float spm;
    private bool beating;
    public event Action OnBeat;
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

        spm = 60f / bpm;
        beating = true;
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
    } 

    private IEnumerator Sequencing()
    {
        while (true)
        {
            _currBeat++;
            OnBeat?.Invoke();
            yield return new WaitForSeconds(spm);      
        }
    }
}
