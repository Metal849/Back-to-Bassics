using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[TrackClipType(typeof(FireProjectileAsset))]
[TrackBindingType(typeof(FireProjectileAction))]
public class FireProjectileTrack : TrackAsset
{
    protected override void OnCreateClip(TimelineClip clip)
    {
        base.OnCreateClip(clip);
        clip.duration = clip.clipAssetDuration;
    }
}
