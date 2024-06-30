using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BroadcastAsset : PlayableAsset
{
    public override double duration => template.broadcastClip == null ? 2f : template.broadcastClip.length;
    public BroadcastBehaviour template;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<BroadcastBehaviour>.Create(graph, template);
        return playable;
    }
}
