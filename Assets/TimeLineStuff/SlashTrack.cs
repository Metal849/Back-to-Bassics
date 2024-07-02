using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(SlashAsset))]
[TrackClipType(typeof(BroadcastAsset))]
[TrackBindingType(typeof(SlashAction))]
public class SlashTrack : TrackAsset
{
    protected override void OnCreateClip(TimelineClip clip)
    {
        var director = TimelineEditor.inspectedDirector;
        SlashAction action = director.GetGenericBinding(this) as SlashAction;
        if (action != null)
        {
            clip.duration = clip.asset.GetType() == typeof(SlashAsset) ? 
                action.preHitClipLengthInBeats : action.broadcastClipLengthInBeats;
        }
        base.OnCreateClip(clip);
    }
}
