using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[TrackClipType(typeof(SlashAsset))]
[TrackClipType(typeof(BroadcastAsset))]
[TrackClipType(typeof(AnimationPlayableAsset))]
[TrackBindingType(typeof(EnemyBattlePawn))]
public class EnemyBattlePawnTrack : TrackAsset
{
    protected override void OnCreateClip(TimelineClip clip)
    {
        base.OnCreateClip(clip);
        clip.duration = clip.clipAssetDuration;
    }
}
