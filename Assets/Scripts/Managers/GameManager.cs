using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameStateMachine GSM { get; private set; }
    public PlayerController PC { get; private set; }

    private void Awake()
    {
        InitializeSingleton();

        // References
        GSM = GetComponent<GameStateMachine>();
        PC = FindObjectOfType<PlayerController>();
    }
}
