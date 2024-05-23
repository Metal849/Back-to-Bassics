using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private PlayerBattlePawn _player;
    [SerializeField] private EnemyBattlePawn _enemy;
    public bool IsBattleActive { get; private set; } 
    public PlayerBattlePawn Player => _player;
    public EnemyBattlePawn Enemy => _enemy;
    private void Awake()
    {
        InitializeSingleton();
    }
    private void Start()
    {
        StartCoroutine(StartBattle());
    }
    private IEnumerator StartBattle()
    {
        _player.EnterBattle();
        yield return new WaitForSeconds(0.4f);
        _enemy.EnterBattle();
        Conductor.Instance.BeginConducting(((EnemyBattlePawnData)_enemy.Data).BPM);
        IsBattleActive = true;
    }
    public void OnPlayerDeath()
    {
        Conductor.Instance.StopConducting();
    }
    public void OnEnemyDeath()
    {
        Conductor.Instance.StopConducting();
    }
}
