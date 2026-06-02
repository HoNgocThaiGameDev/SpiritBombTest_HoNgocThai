using UnityEngine;

public class ObjectPoolerManager : MonoBehaviour
{
    [HideInInspector]
    public ObjectPooler DanDichPooler;
    [HideInInspector]
    public ObjectPooler bullet1_1;
    [HideInInspector]
    public ObjectPooler bullet1_2;
    [HideInInspector]
    public ObjectPooler bulletSP1;
    [HideInInspector]
    public ObjectPooler rocket;

    [Header("Player")]
    public GameObject bullet1Prefab1;
    public GameObject bullet1Prefab2;
    public GameObject bulletSPPrefab1;
    public GameObject rocketPrefabs;

    [Header("Enemy")]
    public GameObject dandich;

    private static ObjectPoolerManager instance;
    private bool initialized;

    public static ObjectPoolerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ObjectPoolerManager>();
            }

            if (instance != null)
            {
                instance.EnsureInitialized();
            }

            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnsureInitialized();
    }

    public void EnsureInitialized()
    {
        if (initialized)
        {
            return;
        }

        rocket = CreatePool("MissilePooler", rocketPrefabs, 8);
        bullet1_1 = CreatePool("Bullet1Level1Pooler", bullet1Prefab1, 30);
        bullet1_2 = CreatePool("Bullet1UpgradePooler", bullet1Prefab2, 30);
        bulletSP1 = CreatePool("SupportBullet1Pooler", bulletSPPrefab1, 30);
        DanDichPooler = CreatePool("EnemyBulletPooler", dandich, 30);

        initialized = true;
    }

    private ObjectPooler CreatePool(string poolName, GameObject prefab, int poolLength)
    {
        if (prefab == null)
        {
            Debug.LogError("Missing prefab for " + poolName, this);
            return null;
        }

        GameObject go = new GameObject(poolName);
        ObjectPooler pooler = go.AddComponent<ObjectPooler>();
        pooler.PooledObject = prefab;
        pooler.PoolLength = poolLength;
        go.transform.parent = transform;
        pooler.Initialize();
        return pooler;
    }
}
