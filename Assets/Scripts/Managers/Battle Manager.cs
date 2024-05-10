using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //The entities that are fighting on the stage
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject conductor;
    [SerializeField] private Player player;

    private int streak = 0;
    private Direction neededResponse = Direction.None;
    private Direction nextNeededResponse = Direction.None;
    private bool playerHasResponded = false;

    void Awake() {
        conductor.SetActive(true);
        conductor.GetComponent<Conductor>().startTempo(enemy.BPM);
    }

    

    public void newBeat() {
        if (enemy == null) {
            conductor.SetActive(false);
        }
        // if (!playerHasResponded && ) {
        //     player.takeDamage();
        //     print(streak);
        //     streak = 0;
        // }
        neededResponse = nextNeededResponse;
        nextNeededResponse = enemy.getIntent();
        // print("NeededResponse = " + neededResponse + ", NextNeededResponse = " + nextNeededResponse);
    }

    public void playerResponse(Direction playerDir) {
        if (enemy.HP <= 0) {
            return;
        }
        if (enemy.SP <= 0) {
            enemy.dealDamage();
            return;
        }
        if (playerDir == neededResponse) {
            // print("Player Response = " + playerDir + ", Enemy Response = " + neededResponse);
            enemy.dealDamage();
            streak++;
            if (streak > 2) {
                print("Great! " + streak);
            }
        } else {
            player.takeDamage();
            streak = 0;
        }
    }

    
}
