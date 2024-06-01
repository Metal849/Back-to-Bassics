using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public bool IsBattleActive { get; private set; } 
    public PlayerBattlePawn Player { get; set; }
    public EnemyBattlePawn Enemy { get; set; }
    private float battleDelay = 3f;
    private void Awake()
    {
        InitializeSingleton();
    }
    public void Start()
    {
        Player = GameManager.Instance.PC.GetComponent<PlayerBattlePawn>();
    }
    public void StartBattle()
    {
        Enemy = Player.CurrEnemyOpponent;
        if (Enemy == null)
        {
            Debug.LogError("BattleManager tried to start battle, but player has no Enemy Opponent!");
            return;
        }
        StartCoroutine(IntializeBattle());
    }
    public void EndBattle()
    {
        IsBattleActive = false;
        Conductor.Instance.StopConducting();

        // Instead of directly to world traversal, need a win screen of some kind
        GameManager.Instance.GSM.Transition<GameStateMachine.WorldTraversal>();
    }
    private IEnumerator IntializeBattle()
    {
        Player.EnterBattle();
        yield return new WaitForSeconds(0.4f);
        Enemy.EnterBattle();
        for (float i = battleDelay; i > 0; i--)
        {
            UIManager.Instance.UpdateCenterText(i.ToString());
            yield return new WaitForSeconds(1f);
        }
        UIManager.Instance.UpdateCenterText("Battle!");
        yield return new WaitForSeconds(1f);
        UIManager.Instance.UpdateCenterText("");
        Conductor.Instance.BeginConducting(((EnemyBattlePawnData)Enemy.Data).BPM);
        IsBattleActive = true;
    }
    public void OnPlayerDeath()
    {
        EndBattle();
        UIManager.Instance.UpdateCenterText("Player Is Dead, SAD!");
    }
    public void OnEnemyDeath()
    {
        EndBattle();
        StartCoroutine(EnemyDefeatTemp());
    } 

    private IEnumerator EnemyDefeatTemp()
    {
        UIManager.Instance.UpdateCenterText($"Defeated {Enemy.Data.Name}!");
        yield return new WaitForSeconds(3f);
        UIManager.Instance.UpdateCenterText("");
    }
}
