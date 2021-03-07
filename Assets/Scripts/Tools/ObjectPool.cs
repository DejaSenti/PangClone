using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectPool<T> where T : PoolableObject
{
    public int ActiveCount { get => activeObjects.Count; }

    private List<T> objectPool;
    private List<T> activeObjects;
    private GameObject gameObject;

    public ObjectPool(GameObject gameObject)
    {
        objectPool = new List<T>();
        activeObjects = new List<T>();
        this.gameObject = gameObject;
    }

    public void Initialize(int poolSize)
    {
        if (objectPool.Count > 0)
        {
            Terminate();
        }

        for (int i = 0; i < poolSize; i++)
        {
            var gameObject = Object.Instantiate(this.gameObject);

            T poolableObject = gameObject.GetComponentInChildren<T>();

            poolableObject.Deactivate();

            objectPool.Add(poolableObject);
        }
    }

    public List<T> GetAllPooledObjects()
    {
        var result = objectPool.Concat(activeObjects).ToList();
        return result;
    }

    public T Acquire()
    {
        if (objectPool.Count == 0)
            return null;

        T spawnedObject = objectPool[0];

        objectPool.RemoveAt(0);
        activeObjects.Add(spawnedObject);

        spawnedObject.Activate();

        return spawnedObject;
    }

    public void Release(T existingObject)
    {
        if (!activeObjects.Contains(existingObject))
            return;

        activeObjects.RemoveAt(activeObjects.IndexOf(existingObject));
        objectPool.Add(existingObject);

        existingObject.Deactivate();
    }

    public void ReleaseAll()
    {
        var allObjects = GetAllPooledObjects();

        foreach(var pooledObject in allObjects)
        {
            Release(pooledObject);
        }
    }

    public void Terminate()
    {
        var allObjects = GetAllPooledObjects();

        objectPool.Clear();
        activeObjects.Clear();

        foreach (T poolableObject in allObjects)
        {
            poolableObject.Deactivate();
            Object.Destroy(poolableObject.gameObject);
        }
    }
}