using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class AudioManager : Singleton<AudioManager>
{
    private void Awake()
    {
        InitializeSingleton();
    }

    // (RYAN) THIS IS TEMP PLEASE DON"T ACTUALLY DO THIS!!!
    public void PlayOnShotSound(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
}
