using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly object SyncRoot = new object();
    private static T instance;
    private static bool applicationIsQuitting;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
                return null;

            lock (SyncRoot)
            {
                if (instance == null)
                    instance = FindExistingOrCreate();

                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        RegisterInstance(this as T);
    }

    protected virtual void OnDestroy()
    {
        if (ReferenceEquals(instance, this))
        {
            instance = null;
            applicationIsQuitting = true;
        }
    }

    private static void RegisterInstance(T target)
    {
        if (target == null)
            return;

        if (instance != null && !ReferenceEquals(instance, target))
        {
            Destroy(target.gameObject);
            return;
        }

        instance = target;
        DontDestroyOnLoad(target.gameObject);
    }

    private static T FindExistingOrCreate()
    {
        T existing = FindObjectOfType<T>();
        if (existing != null)
        {
            RegisterInstance(existing);
            return existing;
        }

        GameObject singleton = new GameObject(typeof(T).Name);
        T created = singleton.AddComponent<T>();
        RegisterInstance(created);
        return created;
    }
}
