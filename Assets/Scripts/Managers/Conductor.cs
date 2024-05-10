using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Conductor : MonoBehaviour
{
    //Variables that help manage the Rhyth, of the game
    private int beat = 0; //The current beat we are on;
    private float time = 0; //The current time of the clock;
    float spb; //The speed that each beat will be played at

    //Variables to manage the 
    [SerializeField] private BattleManager stage;

    public void startTempo(int bpm) {
        spb = 60f / bpm;
    }

    private void Start()
    {
        Time.fixedDeltaTime = spb;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stage.newBeat();
    }
}
