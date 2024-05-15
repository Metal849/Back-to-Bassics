using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : Conductable
{
    #region Singleton
    private static Pooler instance;
    public static Pooler Instance 
    {  
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Pooler>();
            }
            return instance;
        }
    }
    #endregion
    // Temp stuff to play around with spawning

    private Dictionary<Projectile, Projectile[]> _projectileRefMapping;
    private Dictionary<Projectile, int> _projectileSpawnIndex;

    private void Awake()
    {
        _projectileRefMapping = new Dictionary<Projectile, Projectile[]>();
        _projectileSpawnIndex = new Dictionary<Projectile, int>();
    }

    public void InstantiateProjectiles(GameObject projectileRef, int count)
    {
        var projectile = projectileRef.GetComponent<Projectile>();
        if (projectile == null) 
        {
            Debug.LogError("Pooler cannot instantiate object without Projectile component.");
            return;
        }
        _projectileRefMapping[projectile] = new Projectile[count];
        _projectileSpawnIndex[projectile] = 0;
        for (int i = 0; i < count; i++)
        {
            _projectileRefMapping[projectile][i] = Instantiate(projectileRef).GetComponent<Projectile>();
            _projectileRefMapping[projectile][i].transform.SetParent(transform, true);
        }
    }
    public void Spawn(Projectile projectile, Vector3 position, Vector3 velocity)
    {
        // If projectile not destroyed then we looped back to max in use projectiles, thus don't spawn a new one
        if (!_projectileRefMapping[projectile][_projectileSpawnIndex[projectile]].isDestroyed) return;
        _projectileRefMapping[projectile][_projectileSpawnIndex[projectile]].Spawn(position, velocity);
        _projectileSpawnIndex[projectile] = (_projectileSpawnIndex[projectile] + 1) % _projectileRefMapping[projectile].Length;
    }
}
