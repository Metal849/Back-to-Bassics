using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SlashNodeAsset : PlayableAsset
{
    //[SerializeField] private SlashNode slashNode;
    // This dynamical changes depending on deflected or completion
    //public override double duration => slashNode.preHitClip.length; <-- NOT HERE, GO TO TRACK ASSET FOR THIS
    public SlashNodeBehaviour template;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SlashNodeBehaviour>.Create(graph, template); 
        return playable;
    }
    
}
