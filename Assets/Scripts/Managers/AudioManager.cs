using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : Singleton<AudioManager>
{
    private void Awake()
    {
        InitializeSingleton();
    }

    // (RYAN) THIS IS TEMP PLEASE DON"T ACTUALLY DO THIS!!!
    public void PlayPlayerSlash(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
