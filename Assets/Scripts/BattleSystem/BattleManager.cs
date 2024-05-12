using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private PlayerBattlePawn _player;
    [SerializeField] private EnemyBattlePawn _enemy;
    private void Awake()
    {
        InitializeSingleton();
    }
    private void Start()
    {
        StartCoroutine(StartBattle());
        Conductor.Instance.OnBeat += OnBeat;
    }
    private IEnumerator StartBattle()
    {
        _player.EnterBattle();
        yield return new WaitForSeconds(0.4f);
        _enemy.EnterBattle();
        Conductor.Instance.BeginBeating(((EnemyBattlePawnData)_enemy.Data).BPM);
    }
    private void OnBeat()
    {

    }
}
