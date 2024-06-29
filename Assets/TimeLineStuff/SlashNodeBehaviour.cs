using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SlashNodeBehaviour : PlayableBehaviour
{
    public SlashNode node;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        BattleManager.Instance.Enemy.GetEnemyAction<SlashAction>().Slash(node);
    }
}
