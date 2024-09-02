using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyActionAsset : PlayableAsset
{
    public EnemyActionBehaviour template;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<EnemyActionBehaviour>.Create(graph, template);
        return playable;
    }
}
