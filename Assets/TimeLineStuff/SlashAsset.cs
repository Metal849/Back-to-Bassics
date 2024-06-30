using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SlashAsset : PlayableAsset
{
    public override double duration => template.node.preHitClip == null ? 2f : template.node.preHitClip.length;
    public SlashBehaviour template;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SlashBehaviour>.Create(graph, template); 
        return playable;
    }
}
