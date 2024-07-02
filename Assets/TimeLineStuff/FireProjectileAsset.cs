using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FireProjectileAsset : PlayableAsset
{
    public FireProjectileBehaviour template;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<FireProjectileBehaviour>.Create(graph, template);
        return playable;
    }
}
