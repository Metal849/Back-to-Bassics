using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public float HP;
    public float SP;
    public int bpm;
    private Direction intent;
    [SerializeField] private Direction[] pattern = { Direction.North, Direction.None, Direction.East, Direction.None,  Direction.West, Direction.None};
    //Direction.None, Direction.None, Direction.None, Direction.None,
    [SerializeField] GameObject weapon;
    // private int pos = 0;
    private int tick = 0;
    [SerializeField] Animator animator;



    void Start() {
        animator.SetFloat("Time", 60f / bpm);
    }
    
    int delay = 0;
    public Direction getIntent() {
        if (SP <= 0 || HP <= 0) {
            tick++;
            if (tick == 5) {
                SP = 30;
                tick = 0;
            }
            return Direction.None;
        }
        if (delay > 0) {
            delay--;
            return Direction.None;
        }
        
        // if (pos % 2 == 0) {
        //     if (transform.rotation.eulerAngles.y % 360 == 0) {
        //         animator.Play("Spin1", 1, 0f);
        //     } else {
        //         animator.Play("Spin2", 1, 0f);
        //     }
        // }
        // animator.Play("Bounce", 0, 0f);
        int pos = Random.Range(1, 5);
        Direction dir = Direction.None;
        switch(pos) {
            case 1:
                animator.Play("AttackDown", 0, 0f);
                dir = Direction.North;
                // delay = 1;
                break;
            case 2:
                animator.Play("AttackLeft", 0, 0f);
                dir = Direction.East;
                // delay = 1;
                break;
            case 3:
                animator.Play("AttackRight", 0, 0f);
                dir = Direction.West;
                // delay = 1;
                break;
            default:
                animator.Play("Bounce", 0, 0f);
                break;
        }            
        return dir;
    }

    public void dealDamage() {
        if (SP > 0) {
            SP -= 3;
            animator.Play("Staggered", 0, 0f);
        } else {
            HP -=3;
            if (HP <= 0) {
                StartCoroutine(death());
            }
        }
    }

    IEnumerator death() {
        animator.Play("Death", 0, 0f);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
