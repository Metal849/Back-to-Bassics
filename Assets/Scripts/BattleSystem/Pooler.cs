using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : Conductable
{
    // Temp stuff to play around with spawning
    [SerializeField] private Projectile _projectileRef; 
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private int _beatsToReachTarget;
    protected override void OnFullBeat()
    {
        if (!_projectileRef.isDestroyed) return;
        // Thank you Physics 1
        Vector2 r = _target.position - _spawnLocation.position;
        Vector2 v = r / (_beatsToReachTarget * Conductor.Instance.spb);
        _projectileRef.Spawn(_spawnLocation.position, v);
    }
}
