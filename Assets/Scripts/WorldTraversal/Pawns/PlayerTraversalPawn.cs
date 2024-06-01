using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraversalPawn : TraversalPawn
{
    [Header("PlyaerTraversalPawn Specs")]
    [SerializeField] private float battlePositionOffest = -1.8f;
    public void Slash(Vector2 slashDirection)
    {
        Debug.Log($"Traversal Slash {DirectionHelper.GetVectorDirection(slashDirection)}");
    }
    // This will start a battle
    public void EngageEnemy(EnemyBattlePawn enemy)
    {
        GetComponent<PlayerBattlePawn>().CurrEnemyOpponent = enemy;
        MoveToDestination(new Vector2(enemy.transform.position.x, enemy.transform.position.z + battlePositionOffest));
        StartCoroutine(WaitTillPlayerIsInBattleDistance());
    }
    private IEnumerator WaitTillPlayerIsInBattleDistance()
    {
        yield return new WaitUntil(() => !movingToDestination);
        GameManager.Instance.GSM.Transition<GameStateMachine.Battle>();
    }
}
