using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameConfigService : MonoBehaviour
{
    private const string DatabaseResourcePath = "Config/GameConfigDatabase";
    private const string PlayerPlaneResourcePath = "Config/Planes/Plane1";

    private static GameConfigService instance;

    [SerializeField] private GameConfigDatabaseSO database;
    private PlayerPlaneConfigSO playerPlane;

    public static GameConfigService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameConfigService>();
                if (instance == null)
                {
                    GameObject serviceObject = new GameObject(nameof(GameConfigService));
                    instance = serviceObject.AddComponent<GameConfigService>();
                }
            }

            instance.Initialize();
            return instance;
        }
    }

    private bool initialized;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    public PlayerPlaneConfigSO GetPlayerPlane()
    {
        Initialize();
        return playerPlane;
    }

    public UpgradeSOData GetPlayerPlaneUpgrade(int level)
    {
        PlayerPlaneConfigSO plane = GetPlayerPlane();
        return GetByLevel(plane != null ? plane.upgrade : null, level, "player plane");
    }

    public int GetPlayerPlaneUpgradeCount()
    {
        PlayerPlaneConfigSO plane = GetPlayerPlane();
        return plane != null && plane.upgrade != null ? plane.upgrade.Count : 0;
    }

    public MissileSOData GetMissileUpgrade(int level)
    {
        Initialize();
        return GetByLevel(database.powerUpUpgrades != null ? database.powerUpUpgrades.missileUpgrades : null, level, "missile");
    }

    public SupportSOData GetSupportUpgrade(int level)
    {
        Initialize();
        return GetByLevel(database.powerUpUpgrades != null ? database.powerUpUpgrades.supportUpgrades : null, level, "support");
    }

    public ShieldSOData GetShieldUpgrade(int level)
    {
        Initialize();
        return GetByLevel(database.powerUpUpgrades != null ? database.powerUpUpgrades.shieldUpgrades : null, level, "shield");
    }

    public EnemyConfigData GetEnemy(EnemyType type)
    {
        Initialize();
        if (database.enemyCatalog != null && database.enemyCatalog.enemies != null)
        {
            for (int i = 0; i < database.enemyCatalog.enemies.Count; i++)
            {
                EnemyConfigData enemy = database.enemyCatalog.enemies[i];
                if (enemy != null && enemy.enemyType == type)
                {
                    return enemy;
                }
            }
        }

        throw new InvalidOperationException("Missing enemy config for " + type + ".");
    }

    public LevelConfigSO GetLevel(int levelNumber)
    {
        Initialize();
        LevelConfigSO level = database.GetLevel(levelNumber);
        if (level == null)
        {
            throw new InvalidOperationException("Missing level config for level " + levelNumber + ".");
        }

        return level;
    }

    public int GetLevelEnergyCost(int levelNumber)
    {
        LevelConfigSO level = GetLevel(levelNumber);
        return Mathf.Max(0, level.energy);
    }

    public PathConfigData GetFormationPath(int pathId)
    {
        Initialize();
        if (database.formationPaths != null && database.formationPaths.paths != null)
        {
            for (int i = 0; i < database.formationPaths.paths.Count; i++)
            {
                PathConfigData path = database.formationPaths.paths[i];
                if (path != null && path.pathId == pathId)
                {
                    return path;
                }
            }
        }

        throw new InvalidOperationException("Missing formation path config for path " + pathId + ".");
    }

    private void Initialize()
    {
        if (initialized)
        {
            return;
        }

        if (database == null)
        {
            database = Resources.Load<GameConfigDatabaseSO>(DatabaseResourcePath);
        }

        if (database == null)
        {
            throw new InvalidOperationException("Missing Resources/" + DatabaseResourcePath + ".");
        }

        playerPlane = database.playerPlane != null
            ? database.playerPlane
            : Resources.Load<PlayerPlaneConfigSO>(PlayerPlaneResourcePath);

        ValidateDatabase();
        initialized = true;
    }

    private void ValidateDatabase()
    {
        if (playerPlane == null)
        {
            throw new InvalidOperationException("Missing player plane config.");
        }

        if (database.powerUpUpgrades == null)
        {
            throw new InvalidOperationException("Missing power-up upgrade config");
        }

        if (database.enemyCatalog == null || database.enemyCatalog.enemies == null || database.enemyCatalog.enemies.Count == 0)
        {
            throw new InvalidOperationException("Enemy catalog must contain enemy configs.");
        }

        ValidateRequiredEnemyTypes(database.enemyCatalog.enemies);

        if (database.formationPaths == null || database.formationPaths.paths == null || database.formationPaths.paths.Count == 0)
        {
            throw new InvalidOperationException("Missing formation path catalog.");
        }

        if (database.levels == null || database.levels.Count == 0)
        {
            throw new InvalidOperationException("Game config must contain level configs.");
        }

        for (int levelNumber = 1; levelNumber <= 3; levelNumber++)
        {
            if (database.GetLevel(levelNumber) == null)
            {
                throw new InvalidOperationException("Game config is missing required demo level " + levelNumber + ".");
            }
        }
    }

    private static void ValidateRequiredEnemyTypes(List<EnemyConfigData> enemies)
    {
        bool hasBasic = false;
        bool hasHeavy = false;
        bool hasBoss = false;

        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyConfigData enemy = enemies[i];
            if (enemy == null)
                continue;

            hasBasic |= enemy.enemyType == EnemyType.Basic;
            hasHeavy |= enemy.enemyType == EnemyType.Heavy;
            hasBoss |= enemy.enemyType == EnemyType.Boss;
        }

        if (!hasBasic || !hasHeavy || !hasBoss)
        {
            throw new InvalidOperationException("Enemy catalog must contain Basic, Heavy, and Boss configs.");
        }
    }

    private static T GetByLevel<T>(System.Collections.Generic.List<T> values, int level, string configName) where T : class
    {
        int index = level - 1;
        if (values == null || index < 0 || index >= values.Count || values[index] == null)
        {
            throw new InvalidOperationException("Missing " + configName + " config for level " + level + ".");
        }

        return values[index];
    }
}
