using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class FireProjectileBehaviour : PlayableBehaviour
{
    public Direction fireDirection;
    public GameObject projectileRef;
    private bool _performed;
    private EnemyBattlePawn _cachedEnemyRef;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        if (_performed) return;
        _performed = true;
        _cachedEnemyRef = info.output.GetUserData() as EnemyBattlePawn;
        if (_cachedEnemyRef == null)
        {
            //Debug.LogError($"{this} Node did not perform slash animation, enemy reference was null");
            return;
        }
        Vector3 firePosition = -DirectionHelper.GetVectorFromDirection(fireDirection);
        Debug.Log((float)playable.GetDuration());
        _cachedEnemyRef.GetEnemyAction<FireProjectileAction>().FireProjectileAtPlayer(projectileRef, (float)playable.GetDuration(), firePosition);
    }
}
