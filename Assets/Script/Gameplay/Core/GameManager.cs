using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class GameManager : MonoBehaviour
{

    static GameManager instance;
    private bool initialized;
    private bool initializationFailed;
    //public EnemyControl enemyControl;
    public static bool isDestroyAll = false;
    public float speedGold;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            if (instance != null)
            {
                instance.EnsureInitialized();
            }
            return instance;
        }
    }

    public List<EnemyControl> listEnemy;
    public ObjectPooling<EnemyControl> enemyPool;

    public GameObject trapRocketPrefab;
    public ObjectPooling<TrapRocketController> trapRocketPool;

    public GameObject bigRocketPrefab;
    public ObjectPooling<BigRocket> bigRocketPool;

    public Transform itemGroup;
    public GameObject itemPrefab;
    public ItemType[] listItemType;
    public ObjectPooling<Itemcontroller> itemPool;

    public Transform notiGroup;
    public GameObject notiPrefab;
    public Sprite[] NotiTextList;
    public ObjectPooling<TestEffText> NotiPool;

    public Transform scoreGroup;
    public GameObject scrorePrefab;
    public ObjectPooling<ShowScoreText> scoreAddedPool;

    public Transform explosionGroup;
    public Transform goldGroup;

    public GameObject itemCollisonPrefab;
    public ObjectPooling<ItemCollisEffect> itemCollisonPool;

    public GameObject explosionE01Prefab;
    public ObjectPooling<ExplosionControl> explosionE01Pool;

    public GameObject explosionFix1Prefab;
    public ObjectPooling<ExplosionControl> explosionFix1Pool;

    public GameObject explosionFix2Prefab;
    public ObjectPooling<ExplosionControl> explosionFix2Pool;

    public GameObject explosionBossPrefab;
    public ObjectPooling<ExplosionControl> explosionBossPool;

    public GameObject explosion_AllPrefab;
    public ObjectPooling<ExplosionControl> explosion_AllPool;

    public List<CoinEffControl> listCoin;
    public GameObject goldPrefab;
    public ObjectPooling<CoinEffControl> goldPool;

    public GameObject corePrefab1;
    public ObjectPooling<CoreController> corePool1;
    public GameObject corePrefab2;
    public ObjectPooling<CoreController> corePool2;
    public GameObject corePrefab3;
    public ObjectPooling<CoreController> corePool3;

    public Transform BulletGroup;
    public Transform trapGroup;


    public GameObject objX2Coin;

    void Awake()
    {
        instance = this;
        EnsureInitialized();
    }

    public void EnsureInitialized()
    {
        if ((initialized && enemyPool != null) || initializationFailed)
        {
            return;
        }

        if (!ValidateRequiredReferences())
        {
            initializationFailed = true;
            enabled = false;
            return;
        }

        Application.targetFrameRate = 60;
        enemyPool = new ObjectPooling<EnemyControl>(40, ResetEnemy, InitEnemy);
        for (int i = 0; i < listEnemy.Count; i++)
        {
            enemyPool.Store(listEnemy[i]);
        }

        if (trapRocketPrefab != null)
        {
            trapRocketPool = new ObjectPooling<TrapRocketController>(6, null, InitTrapRocket);
            trapRocketPool.InitBuffer(6);
        }
        bigRocketPool = new ObjectPooling<BigRocket>(6, null, InitBigRocket);
        bigRocketPool.InitBuffer(6);

        itemPool = new ObjectPooling<Itemcontroller>(3, ResetItem, InitItem);

        Itemcontroller[] itemPreInit = itemGroup.GetComponentsInChildren<Itemcontroller>();
        for (int i = 0; i < itemPreInit.Count(); i++)
        {
            itemPreInit[i].gameObject.SetActive(false);
            //itemPool.Store(itemPreInit[i]);
        }

        NotiPool = new ObjectPooling<TestEffText>(1, ResetNoti, InitNoti);
        TestEffText[] notiPreInit = notiGroup.GetComponentsInChildren<TestEffText>();
        for (int i = 0; i < notiPreInit.Count(); i++)
        {
            notiPreInit[i].gameObject.SetActive(false);
            //itemPool.Store(itemPreInit[i]);
        }

        //scoreAddedPool = new ObjectPooling<ShowScoreText>(10, null, InitAddScore);
        //ShowScoreText[] scorePreInit = scoreGroup.GetComponentsInChildren<ShowScoreText>();
        //for (int i = 0; i < scorePreInit.Count(); i++)
        //{
        //    scorePreInit[i].gameObject.SetActive(false);
        //    //itemPool.Store(itemPreInit[i]);
        //}
        scoreAddedPool = new ObjectPooling<ShowScoreText>(15, null, InitAddScore);
        scoreAddedPool.InitBuffer(15);

        itemCollisonPool = new ObjectPooling<ItemCollisEffect>(2, null, InitItemCollisEffect);
        itemCollisonPool.InitBuffer(2);

        explosionE01Pool = new ObjectPooling<ExplosionControl>(7, null, InitExplosionE01);
        explosionE01Pool.InitBuffer(5);
        explosionFix1Pool = new ObjectPooling<ExplosionControl>(7, null, InitExplosionFix1);
        explosionFix1Pool.InitBuffer(7);
        explosionFix2Pool = new ObjectPooling<ExplosionControl>(5, null, InitExplosionFix2);
        explosionFix2Pool.InitBuffer(5);
        explosionBossPool = new ObjectPooling<ExplosionControl>(1, null, InitExplosionBoss);
        explosionBossPool.InitBuffer(1);
        explosion_AllPool = new ObjectPooling<ExplosionControl>(2, null, InitExplosionAll);
        explosion_AllPool.InitBuffer(2);

        goldPool = new ObjectPooling<CoinEffControl>(150, null, InitGold);
        goldPool.InitBuffer(150);

        corePool1 = new ObjectPooling<CoreController>(3, null, InitCore1);
        corePool1.InitBuffer(3);
        corePool2 = new ObjectPooling<CoreController>(3, null, InitCore2);
        corePool2.InitBuffer(3);
        corePool3 = new ObjectPooling<CoreController>(3, null, InitCore3);
        corePool3.InitBuffer(3);


        initialized = true;
    }

    void Start()
    {
        if (GameState.isX2Coin && objX2Coin != null)
        {
            objX2Coin.SetActive(true);
        }
    }


    // Update is called once per frame
    //void Update()
    //{
    //    if (isDestroyAll)
    //    {
    //        for (int i = 0; i < listEnemy.Count; i++)
    //        {
    //            if (listEnemy[i].gameObject.activeInHierarchy)
    //            {
    //                GameState.IncreaseHit();
    //                listEnemy[i].gameObject.SetActive(false);
    //            }
    //            if (i == listEnemy.Count - 1)
    //            {
    //                isDestroyAll = false;
    //            }
    //        }
    //    }
    //}


    void OnApplicationQuit()
    {
        for (int i = 0; i < listEnemy.Count; i++)
        {
            listEnemy[i].gameObject.SetActive(false);
        }
    }

    #region Init & Reset Pool
    private bool ValidateRequiredReferences()
    {
        List<string> missing = new List<string>();
        if (listEnemy == null || listEnemy.Count == 0)
            missing.Add(nameof(listEnemy));
        Require(bigRocketPrefab, nameof(bigRocketPrefab), missing);
        Require(itemGroup, nameof(itemGroup), missing);
        Require(itemPrefab, nameof(itemPrefab), missing);
        Require(notiGroup, nameof(notiGroup), missing);
        Require(notiPrefab, nameof(notiPrefab), missing);
        Require(scoreGroup, nameof(scoreGroup), missing);
        Require(scrorePrefab, nameof(scrorePrefab), missing);
        Require(explosionGroup, nameof(explosionGroup), missing);
        Require(goldGroup, nameof(goldGroup), missing);
        Require(itemCollisonPrefab, nameof(itemCollisonPrefab), missing);
        Require(explosionE01Prefab, nameof(explosionE01Prefab), missing);
        Require(explosionFix1Prefab, nameof(explosionFix1Prefab), missing);
        Require(explosionFix2Prefab, nameof(explosionFix2Prefab), missing);
        Require(explosionBossPrefab, nameof(explosionBossPrefab), missing);
        Require(explosion_AllPrefab, nameof(explosion_AllPrefab), missing);
        Require(goldPrefab, nameof(goldPrefab), missing);
        Require(corePrefab1, nameof(corePrefab1), missing);
        Require(corePrefab2, nameof(corePrefab2), missing);
        Require(corePrefab3, nameof(corePrefab3), missing);
        Require(trapGroup, nameof(trapGroup), missing);
        if (missing.Count == 0)
            return true;

        Debug.LogError("[GameManager] Cannot initialize gameplay pools. Missing required references: " + string.Join(", ", missing.ToArray()), this);
        return false;
    }

    private static void Require(UnityEngine.Object value, string fieldName, List<string> missing)
    {
        if (value == null)
            missing.Add(fieldName);
    }

    void InitEnemy(EnemyControl enemy)
    {
        if (enemy == null)
        {
            enemy = GameplayPoolFactory.CreateInactive<EnemyControl>(
                listEnemy[0].gameObject,
                listEnemy[0].transform.parent,
                listEnemy[0].transform.position,
                nameof(enemyPool));
            this.listEnemy.Add(enemy);
            enemyPool.Store(enemy);
        }
    }

    void InitTrapRocket(TrapRocketController trap)
    {
        if (trap == null)
        {
            trap = GameplayPoolFactory.CreateInactive<TrapRocketController>(
                trapRocketPrefab, trapGroup, trapGroup.position, nameof(trapRocketPool));
            trapRocketPool.Store(trap);
        }
    }

    void InitBigRocket(BigRocket bigRocket)
    {
        if (bigRocket == null)
        {
            bigRocket = GameplayPoolFactory.CreateInactive<BigRocket>(
                bigRocketPrefab, trapGroup, trapGroup.position, nameof(bigRocketPool));
            bigRocketPool.Store(bigRocket);
        }
    }

    void ResetEnemy(EnemyControl enemy)
    {
        if (enemy != null)
        {
            enemy.gameObject.SetActive(true);
        }
    }


    void InitItem(Itemcontroller item)
    {
        if (item == null)
        {
            item = GameplayPoolFactory.CreateInactive<Itemcontroller>(
                itemPrefab, itemGroup, itemGroup.position, nameof(itemPool));
            itemPool.Store(item);
        }
    }
    void ResetItem(Itemcontroller item)
    {
        if (item != null)
        {
            item.gameObject.SetActive(true);
        }
    }
    void InitNoti(TestEffText item)
    {
        if (item == null)
        {
            item = GameplayPoolFactory.CreateInactive<TestEffText>(
                notiPrefab, notiGroup, notiGroup.position, nameof(NotiPool));
            NotiPool.Store(item);
        }
    }
    void ResetNoti(TestEffText item)
    {
        if (item != null)
        {
            item.gameObject.SetActive(true);
        }
    }

    void InitAddScore(ShowScoreText item)
    {
        if (item == null)
        {
            item = GameplayPoolFactory.CreateInactive<ShowScoreText>(
                scrorePrefab, scoreGroup, scoreGroup.position, nameof(scoreAddedPool));
            scoreAddedPool.Store(item);
        }
    }

    void InitItemCollisEffect(ItemCollisEffect effect)
    {
        if (effect == null)
        {
            effect = GameplayPoolFactory.CreateInactive<ItemCollisEffect>(
                itemCollisonPrefab, scoreGroup, scoreGroup.position, nameof(itemCollisonPool));
            itemCollisonPool.Store(effect);
        }
    }

    void InitExplosionE01(ExplosionControl item)
    {
        if (item == null)
        {
            item = GameplayPoolFactory.CreateInactive<ExplosionControl>(
                explosionE01Prefab, explosionGroup, explosionGroup.position, nameof(explosionE01Pool));
            item.type = ExplosionType.ExplosionE01;
            explosionE01Pool.Store(item);
        }
    }

    void InitExplosionFix1(ExplosionControl item)
    {
        if (item == null)
        {
            item = GameplayPoolFactory.CreateInactive<ExplosionControl>(
                explosionFix1Prefab, explosionGroup, explosionGroup.position, nameof(explosionFix1Pool));
            item.type = ExplosionType.ExplosionFix1;
            explosionFix1Pool.Store(item);
        }
    }

    void InitExplosionFix2(ExplosionControl item)
    {
        if (item == null)
        {
            item = GameplayPoolFactory.CreateInactive<ExplosionControl>(
                explosionFix2Prefab, explosionGroup, explosionGroup.position, nameof(explosionFix2Pool));
            item.type = ExplosionType.ExplosionFix2;
            explosionFix2Pool.Store(item);
        }
    }
    void InitExplosionBoss(ExplosionControl item)
    {
        if (item == null)
        {
            item = GameplayPoolFactory.CreateInactive<ExplosionControl>(
                explosionBossPrefab, explosionGroup, explosionGroup.position, nameof(explosionBossPool));
            item.type = ExplosionType.ExplosionBoss;
            explosionBossPool.Store(item);
        }
    }
    void InitExplosionAll(ExplosionControl item)
    {
        if (item == null)
        {
            item = GameplayPoolFactory.CreateInactive<ExplosionControl>(
                explosion_AllPrefab, explosionGroup, explosionGroup.position, nameof(explosion_AllPool));
            item.type = ExplosionType.ExplosionAll;
            explosion_AllPool.Store(item);
        }
    }
    void InitGold(CoinEffControl item)
    {
        if (item == null)
        {
            item = GameplayPoolFactory.CreateInactive<CoinEffControl>(
                goldPrefab, goldGroup, goldGroup.position, nameof(goldPool));
            this.listCoin.Add(item);
            goldPool.Store(item);
        }
    }

    void InitCore1(CoreController core)
    {
        if (core == null)
        {
            core = GameplayPoolFactory.CreateInactive<CoreController>(
                corePrefab1, goldGroup, goldGroup.position, nameof(corePool1));
            corePool1.Store(core);
        }
    }
    void InitCore2(CoreController core)
    {
        if (core == null)
        {
            core = GameplayPoolFactory.CreateInactive<CoreController>(
                corePrefab2, goldGroup, goldGroup.position, nameof(corePool2));
            corePool2.Store(core);
        }
    }
    void InitCore3(CoreController core)
    {
        if (core == null)
        {
            core = GameplayPoolFactory.CreateInactive<CoreController>(
                corePrefab3, goldGroup, goldGroup.position, nameof(corePool3));
            corePool3.Store(core);
        }
    }

    #endregion


}

[Serializable]
public struct ItemType
{
    public string tag;
    public Sprite sprite;
}

