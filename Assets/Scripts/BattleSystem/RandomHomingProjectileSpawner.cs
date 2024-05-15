using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RandomHomingProjectileSpawner : Conductable
{
    [Header("Projectile Spec")]
    [SerializeField] private GameObject _projectileRef;
    [SerializeField] private int _maxProjectileSpawn;
    [SerializeField] private BoxCollider _spawnMedium;
    [Header("Target Spec")]
    [SerializeField] private Transform _target;
    [SerializeField] private int _beatsToReachTarget;

    private Projectile _projectile;
    protected override void Start()
    {
        base.Start();
        _projectile = _projectileRef.GetComponent<Projectile>();
        Pooler.Instance.InstantiateProjectiles(_projectileRef, _maxProjectileSpawn);
    }
    protected override void OnFullBeat()
    {
        Vector2 spawnLocation = new Vector2
            (
                Random.Range(_spawnMedium.bounds.min.x, _spawnMedium.bounds.max.x),
                Random.Range(_spawnMedium.bounds.min.y, _spawnMedium.bounds.max.y)
            );
        // Thank you Physics 1
        Vector2 r = _target.position - (Vector3)spawnLocation;
        Vector2 v = r / (_beatsToReachTarget * Conductor.Instance.spb);
        Pooler.Instance.Spawn(_projectile, spawnLocation, v);
    }
}
