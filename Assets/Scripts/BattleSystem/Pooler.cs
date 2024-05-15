using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : Conductable
{
    [SerializeField] private Projectile _projectileRef;
    protected override void OnBeat()
    {
        if (_projectileRef.isDestroyed) _projectileRef.Spawn(new Vector2(4f, 0.6f), new Vector2(-5f, 0f));
    }
}
