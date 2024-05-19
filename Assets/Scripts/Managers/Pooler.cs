using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Pooler : Singleton<Pooler>
{
    class PooledObject
    {
        public GameObject[] objects { get; set; }
        public int currIdx { get; set; }
    }

    private Dictionary<GameObject, PooledObject> _pooledRefMapping;

    private void Awake()
    {
        InitializeSingleton();
        _pooledRefMapping = new Dictionary<GameObject, PooledObject>();
    }
    /// <summary>
    /// Store and clone a particular prefab to pool
    /// </summary>
    /// <param name="pooledRef"></param>
    /// <param name="count"></param>
    public void PoolGameObject(GameObject pooledRef, int count)
    {
        if (pooledRef == null) 
        {
            Debug.LogError("Pooler cannot instantiate object without Projectile component.");
            return;
        }
        _pooledRefMapping[pooledRef] = new PooledObject();
        _pooledRefMapping[pooledRef].objects = new GameObject[count];
        _pooledRefMapping[pooledRef].currIdx = 0;

        // Pool GameObjects
        for (int i = 0; i < count; i++)
        {
            _pooledRefMapping[pooledRef].objects[i] = Instantiate(pooledRef);
            _pooledRefMapping[pooledRef].objects[i].SetActive(false);
            _pooledRefMapping[pooledRef].objects[i].transform.SetParent(transform, true);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public GameObject Pool(GameObject go)
    {
        int idx = _pooledRefMapping[go].currIdx;
        _pooledRefMapping[go].currIdx = (idx + 1) % _pooledRefMapping[go].objects.Length;
        _pooledRefMapping[go].objects[idx].transform.SetParent(transform, true);
        return _pooledRefMapping[go].objects[idx];
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="globalPosition"></param>
    /// <returns></returns>
    public GameObject Spawn(GameObject go, Vector3 globalPosition)
    {
        go = Pool(go);
        go.transform.position = globalPosition;
        go.SetActive(true);
        return go;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public GameObject NextObjectToPool(GameObject go)
    {
        return _pooledRefMapping[go].objects[_pooledRefMapping[go].currIdx];
    }
}
