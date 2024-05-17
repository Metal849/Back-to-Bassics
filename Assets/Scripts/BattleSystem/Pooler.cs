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
    class ProjectileBank
    {
        public Projectile[] projectiles { get; set; }
        public int currIdx { get; set; }
    }

    private Dictionary<Projectile, ProjectileBank> _projectileRefMapping;

    private void Awake()
    {
        _projectileRefMapping = new Dictionary<Projectile, ProjectileBank>();
    }

    public void InstantiateProjectiles(GameObject projectileRef, int count)
    {
        var projectile = projectileRef.GetComponent<Projectile>();
        if (projectile == null) 
        {
            Debug.LogError("Pooler cannot instantiate object without Projectile component.");
            return;
        }
        _projectileRefMapping[projectile] = new ProjectileBank();
        _projectileRefMapping[projectile].projectiles = new Projectile[count];
        _projectileRefMapping[projectile].currIdx = 0;
        for (int i = 0; i < count; i++)
        {
            _projectileRefMapping[projectile].projectiles[i] = Instantiate(projectileRef).GetComponent<Projectile>();
            _projectileRefMapping[projectile].projectiles[i].transform.SetParent(transform, true);
        }
    }
    public void Spawn(Projectile projectile, Vector3 position, Vector3 velocity)
    {
        // If projectile not destroyed then we looped back to max in use projectiles, thus don't spawn a new one
        if (!_projectileRefMapping[projectile].projectiles[_projectileRefMapping[projectile].currIdx].isDestroyed) return;
        _projectileRefMapping[projectile].projectiles[_projectileRefMapping[projectile].currIdx].Spawn(position, velocity);
        _projectileRefMapping[projectile].currIdx = (_projectileRefMapping[projectile].currIdx + 1) % _projectileRefMapping[projectile].projectiles.Length;
    }
}
