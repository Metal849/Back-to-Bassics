using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class SlashBehaviour : PlayableBehaviour
{
    public SlashNode node;
    private bool _performed;
    private EnemyBattlePawn _cachedEnemyRef;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        if (_performed) return;
        _performed = true;
        _cachedEnemyRef = info.output.GetUserData() as EnemyBattlePawn;
        if (_cachedEnemyRef == null )
        {
            //Debug.LogError($"{this} Node did not perform slash animation, enemy reference was null");
            return;
        }
        _cachedEnemyRef.GetEnemyAction<SlashAction>()?.Slash(node);
    }
}
