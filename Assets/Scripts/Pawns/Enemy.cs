using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Animator _animator;

    private int _currHP;
    private int _currSP;
    private int tick;

    public int HP => _currHP;
    public int SP => _currSP;
    public int BPM => _enemyData.BPM;
    
    void Start() {
        _animator.SetFloat("Time", 60f / _enemyData.BPM);
    }
    
    int delay = 0;
    public Direction getIntent() {
        if (_currSP <= 0 || _currHP <= 0) {
            tick++;
            if (tick == 5) {
                _currSP = 30;
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
                _animator.Play("AttackDown", 0, 0f);
                dir = Direction.North;
                // delay = 1;
                break;
            case 2:
                _animator.Play("AttackLeft", 0, 0f);
                dir = Direction.East;
                // delay = 1;
                break;
            case 3:
                _animator.Play("AttackRight", 0, 0f);
                dir = Direction.West;
                // delay = 1;
                break;
            default:
                _animator.Play("Bounce", 0, 0f);
                break;
        }            
        return dir;
    }

    public void dealDamage() {
        if (_currSP > 0) {
            _currSP -= 3;
            _animator.Play("Staggered", 0, 0f);
        } else {
            _currHP -= 3;
            if (_currHP <= 0) {
                StartCoroutine(death());
            }
        }
    }

    IEnumerator death() {
        _animator.Play("Death", 0, 0f);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
