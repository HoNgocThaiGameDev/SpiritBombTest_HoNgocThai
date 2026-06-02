using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject PooledObject;
    public int PoolLength = 30;

    private readonly List<GameObject> pooledObjects = new List<GameObject>();

    public void Initialize()
    {
        pooledObjects.Clear();
        for (int i = 0; i < PoolLength; i++)
        {
            CreateObjectInPool();
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        int indexToReturn = pooledObjects.Count;
        CreateObjectInPool();
        return pooledObjects[indexToReturn];
    }

    private void CreateObjectInPool()
    {
        GameObject pooledObject = PooledObject == null
            ? new GameObject(name + " PooledObject")
            : Instantiate(PooledObject);

        pooledObject.SetActive(false);
        pooledObject.transform.SetParent(transform);
        pooledObjects.Add(pooledObject);
    }
}
