using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicAttack : EnemyAction
{
    [SerializeField] private Direction[] directions;
    [SerializeField] private GameObject musicNoteRef;
    int currIdx;
    private void Start()
    {
        Pooler.Instance.PoolGameObject(musicNoteRef, 20);
    }
    public override void StartAction()
    {
        base.StartAction();
        currIdx = 0;
        ParentPawn.SpriteAnimator.Play("music");
    }
    protected override void OnFullBeat()
    {
        if (currIdx < directions.Length && IsActive)
        {
            Projectile proj = Pooler.Instance.Pool(musicNoteRef).GetComponent<Projectile>();
            Vector3 directionVector = -(Vector3)DirectionHelper.GetVectorFromDirection(directions[currIdx]);
            ParentPawn.SpriteAnimator.SetFloat("xdir", directionVector.x);
            proj.transform.position = BattleManager.Instance.Player.transform.position + (directionVector) * 6;
            // Take One Beat to fire
            proj.Fire((BattleManager.Instance.Player.transform.position - proj.transform.position)/(2*Conductor.Instance.spb));
            currIdx++;
            if (currIdx >= directions.Length)
            {
                StartCoroutine(WaitForProjectileToDestroy(proj));
                StopAction();
            }
        }
    }
    private IEnumerator WaitForProjectileToDestroy(Projectile proj)
    {
        yield return new WaitUntil(() => proj.isDestroyed);
        StopAction();
    }
}
