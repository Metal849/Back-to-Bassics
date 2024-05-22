using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAttack : EnemyAction
{
    [SerializeField] private Direction[] directions;
    [SerializeField] private GameObject musicNoteRef;
    int currIdx;
    private new void Start()
    {
        base.Start();
        Pooler.Instance.PoolGameObject(musicNoteRef, 20);
    }
    public override void StartAction()
    {
        base.StartAction();
        currIdx = 0;
    }

    protected override void OnFullBeat()
    {
        if (IsActive)
        {
            Projectile proj = Pooler.Instance.Pool(musicNoteRef).GetComponent<Projectile>();
            proj.transform.position = BattleManager.Instance.Player.transform.position + (-(Vector3)DirectionHelper.GetVectorFromDirection(directions[currIdx])) * 6;
            // Take One Beat to fire
            proj.Fire((BattleManager.Instance.Player.transform.position - proj.transform.position)/Conductor.Instance.spb);
            currIdx++;
            if (currIdx >= directions.Length)
            {
                StopAction();
            }
        }
    }
}
