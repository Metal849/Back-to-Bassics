using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class BroadcastAsset : PlayableAsset
{
    public BroadcastBehaviour template;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<BroadcastBehaviour>.Create(graph, template);
        return playable;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BroadcastAsset))]
public class BroadcastAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BroadcastAsset ba = target as BroadcastAsset;
        ba.template.direction = (Direction)EditorGUILayout.EnumPopup("Broadcast Direction", ba.template.direction);
    }
}
#endif
