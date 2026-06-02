using System;
using UnityEngine;

public static class GameplayPoolFactory
{
    private static Transform stagingRoot;

    public static T CreateInactive<T>(GameObject prefab, Transform parent, Vector3 position, string poolName)
        where T : Component
    {
        if (prefab == null)
            throw new InvalidOperationException("Missing prefab for pool '" + poolName + "'.");

        GameObject clone = UnityEngine.Object.Instantiate(prefab, GetStagingRoot());
        clone.SetActive(false);
        clone.transform.SetParent(parent, false);
        clone.transform.position = position;

        T component = clone.GetComponent<T>();
        if (component != null)
            return component;

        UnityEngine.Object.Destroy(clone);
        throw new InvalidOperationException(
            "Prefab '" + prefab.name + "' for pool '" + poolName + "' is missing component " + typeof(T).Name + ".");
    }

    private static Transform GetStagingRoot()
    {
        if (stagingRoot != null)
            return stagingRoot;

        GameObject root = new GameObject("GameplayPoolStaging");
        root.hideFlags = HideFlags.HideAndDontSave;
        root.SetActive(false);
        stagingRoot = root.transform;
        return stagingRoot;
    }
}
