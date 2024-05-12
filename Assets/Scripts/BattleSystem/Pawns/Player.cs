using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float HP = 10;
    public float SP;

    // Singleton
    private static Player thisPlayer;
    public static Player getPlayer() {
        return thisPlayer;
    }

    void Start() {
        if (thisPlayer == null) {
            thisPlayer = this;
        }
    }
    
    [SerializeField] private BattleManager stage;
    private Direction playerDirection = Direction.None;


    //Triggers on Press / Release, returns direction of press.
    private void OnDodge(InputValue input) {
        Vector2 playerInput = input.Get<Vector2>();
        if (playerInput.x == 0 && playerInput.y == 0) {
            return;
        }
        if (playerInput.y > 0) {
            playerDirection = Direction.North;
        } else if (playerInput.y < 0) {
            playerDirection = Direction.South;
        } else if (playerInput.x > 0) {
            playerDirection = Direction.East;
        } else {
            playerDirection = Direction.West;
        }

        //stage.playerResponse(playerDirection);
    }

    private void OnAttack(InputValue input) {
        Vector2 playerInput = input.Get<Vector2>();
        if (playerInput.x == 0 && playerInput.y == 0) {
            return;
        }
        
        if (playerInput.y > 0) {
            playerDirection = Direction.North;
        } else if (playerInput.y < 0) {
            playerDirection = Direction.South;
        } else if (playerInput.x > 0) {
            playerDirection = Direction.East;
        } else {
            playerDirection = Direction.West;
        }

        //stage.playerResponse(playerDirection);
    }

    public void takeDamage() {
        HP -= 1;
    }
    

}
