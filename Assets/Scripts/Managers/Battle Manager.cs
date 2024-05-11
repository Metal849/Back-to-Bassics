using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private PlayerBattlePawn _player;
    [SerializeField] private EnemyBattlePawn _enemy;
    private void Start()
    {
        StartCoroutine(StartBattle());
    }
    private IEnumerator StartBattle()
    {
        _player.EnterBattle();
        yield return new WaitForSeconds(0.4f);
        _enemy.EnterBattle();
    }
}
