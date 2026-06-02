using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityToolbag;
using DG.Tweening;

public class MoveObj
{

    public float timeRun;
    public float timeDelay;
    public int pathType;
    public string pathName;

    EnemyControl enemy;
    public MoveObj(EnemyControl enemy)
    {
        this.enemy = enemy;
    }

    public void Tween()
    {
        Vector3[] path = EnemyPath.GetPath(pathName);
        if (path == null || path.Length < 2)
        {
            if (enemy != null)
                enemy.gameObject.SetActive(false);
            return;
        }

        PlayEnemyPath(path);
    }

    private void PlayEnemyPath(Vector3[] path)
    {
        if (enemy == null || path == null || path.Length < 2)
        {
            return;
        }

        Tween pathTween = enemy.transform.DOPath(ClampPathToVisibleWidth(path), timeRun, PathType.CatmullRom, PathMode.TopDown2D)
            .SetEase(Ease.Linear);

        if (timeDelay <= 0f)
        {
            enemy.SetPathDelayVisible(true);
            return;
        }

        pathTween.Pause();
        enemy.SetPathDelayVisible(false);
        DOTween.Sequence()
            .SetTarget(enemy.transform)
            .AppendInterval(timeDelay)
            .AppendCallback(() => enemy.SetPathDelayVisible(true))
            .Append(pathTween);
    }

    private static Vector3[] ClampPathToVisibleWidth(Vector3[] path)
    {
        Vector3[] clampedPath = new Vector3[path.Length];
        float visibleLimit = GetVisibleXLimit();
        for (int i = 0; i < path.Length; i++)
        {
            clampedPath[i] = path[i];
            clampedPath[i].x = Mathf.Clamp(clampedPath[i].x, -visibleLimit, visibleLimit);
        }

        return clampedPath;
    }

    private static float GetVisibleXLimit()
    {
        Camera camera = Camera.main;
        if (camera == null || !camera.orthographic)
            return 1.8f;

        const float horizontalPadding = 0.35f;
        float halfWidth = camera.orthographicSize * camera.aspect;
        return Mathf.Max(0.45f, halfWidth - horizontalPadding);
    }

}




public class EnemyControl : CacheBehaviour
{
    //public static EnemyControl instance;
    public int idEnemy;
    public EnemyMovementMode movementMode = EnemyMovementMode.FollowPath;
    public float speed;
    public Vector3 pos;
    public int idItemDrop = 0;
    public int heath;     // so mau
    public int currentHP; // so mau con lai
    public int atk;         // dmg
    public int def;         // so shield - ref den  than giap tren UI
    public int eng;         // nang luong (mau)- ref den thanh mau tren UI
    public int score;       // so diem duoc them
    public int gold;        // no ra  gold
    int rand;               // so coin
    int randomAttack;
    Vector3 showPos;
    Vector3 textPos;
    bool onStayDmg;
    bool isPause;
    //[HideInInspector]
    //public Sprite mainSprite;
    public GameObject hpBar;
    public Transform hpTrans;
    private Vector3 hpScale;
    public SpriteRenderer spriteRenderer;
    //Sprite currentSprite;
    float nextFire = 2;
    float fireRate = 2f;
    int rate;
    public Transform spawnPos;
    public List<Sprite> listSprite;
    private const int VisibleSortingOrder = 20;
    private Color colorTakeDmg;
    private Color colorCurrent;
    MoveObj moveObj;
    private WaitForSeconds waitReturnColor;
    private WaitForSeconds waitStayDmg;
    private WaitForSeconds wait6;
    private bool hideForPathDelay;
    private bool hasScreenRetreated;
    private bool isScreenRetreating;
    private bool canShoot;

    private float waitDontShot = 0;
    Vector3 rotateBullet;
    EnemyPath iPath;
    public MoveObj MoveObj
    {
        get { return this.moveObj; }
        set { this.moveObj = value; }
    }
    public override void Awake()
    {
        base.Awake();
        colorTakeDmg = new Color(1f, 1f, 0f, 0.8f);
        colorCurrent = new Color(1f, 1f, 1f, 1f);
        waitReturnColor = new WaitForSeconds(0.15f);
        waitStayDmg = new WaitForSeconds(0.3f);
        wait6 = new WaitForSeconds(3f);
        EnsureSpriteRenderer();
    }

    void OnEnable()
    {
        if (WaveManager.instance != null)
        {
            WaveManager.instance.countEnemy += 1;
        }
        if (moveObj == null)
            moveObj = new MoveObj(this);
        EnsureVisibleRenderer();
        hpBar.SetActive(false);
        rand = Random.Range(0, 3);
        randomAttack = 1;
        nextFire = Time.time;

        onStayDmg = false;
        hideForPathDelay = false;
        hasScreenRetreated = false;
        isScreenRetreating = false;
        canShoot = true;
    }

    public void StartHide()
    {
        StartCoroutine(HideEnemy());
    }

    public void MoveToFormationHold(Vector3 spawnPosition, Vector3 targetPosition, float moveDuration, float holdDuration, float spawnDelay)
    {
        if (transformH == null)
        {
            return;
        }

        transformH.position = ClampToVisibleWidth(spawnPosition);
        if (moveObj == null)
            moveObj = new MoveObj(this);

        moveObj.timeDelay = spawnDelay;
        moveObj.timeRun = moveDuration;
        StartCoroutine(FormationHoldRoutine(targetPosition, moveDuration, holdDuration, spawnDelay));
    }

    public void MoveSineHorizontal(Vector3 centerPosition, float targetY, float verticalSpeed, float duration, float amplitude, float frequency, float phaseOffset, float spawnDelay)
    {
        if (transformH == null)
        {
            return;
        }

        Vector3 spawnPosition = centerPosition;
        spawnPosition.x += Mathf.Sin(phaseOffset) * amplitude;
        transformH.position = ClampToVisibleWidth(spawnPosition);
        if (moveObj == null)
            moveObj = new MoveObj(this);

        moveObj.timeDelay = spawnDelay;
        moveObj.timeRun = duration;
        StartCoroutine(SineHorizontalRoutine(centerPosition, targetY, verticalSpeed, duration, amplitude, frequency, phaseOffset, spawnDelay));
    }

    public void MoveCircleFormation(Vector3 spawnPosition, Vector3 center, float radius, float startAngle, float moveDuration, float rotationSpeed, float spawnDelay)
    {
        if (transformH == null)
        {
            return;
        }

        transformH.position = ClampToVisibleWidth(spawnPosition);
        if (moveObj == null)
            moveObj = new MoveObj(this);

        moveObj.timeDelay = spawnDelay;
        moveObj.timeRun = moveDuration;
        StartCoroutine(CircleFormationRoutine(center, radius, startAngle, moveDuration, rotationSpeed, spawnDelay));
    }

    private IEnumerator FormationHoldRoutine(Vector3 targetPosition, float moveDuration, float holdDuration, float spawnDelay)
    {
        randomAttack = 0;
        if (spawnDelay > 0f)
        {
            SetPathDelayVisible(false);
            yield return new WaitForSeconds(spawnDelay);
        }

        SetPathDelayVisible(true);
        Tween moveTween = transformH.DOMove(ClampToVisibleWidth(targetPosition), moveDuration).SetEase(Ease.Linear);
        yield return moveTween.WaitForCompletion();
        randomAttack = 1;
    }

    private IEnumerator SineHorizontalRoutine(Vector3 centerPosition, float targetY, float verticalSpeed, float duration, float amplitude, float frequency, float phaseOffset, float spawnDelay)
    {
        randomAttack = 0;
        if (spawnDelay > 0f)
        {
            SetPathDelayVisible(false);
            yield return new WaitForSeconds(spawnDelay);
        }

        SetPathDelayVisible(true);
        randomAttack = 0;
        float elapsed = 0f;
        while (!GameState.won)
        {
            elapsed += Time.deltaTime;
            float x = ClampToVisibleWidth(centerPosition.x + Mathf.Sin(elapsed * frequency + phaseOffset) * amplitude);
            float y = centerPosition.y - verticalSpeed * elapsed;
            if (y <= targetY)
            {
                transformH.position = new Vector3(x, targetY, centerPosition.z);
                break;
            }

            transformH.position = new Vector3(x, y, centerPosition.z);

            if (elapsed >= duration)
                break;

            yield return null;
        }

        randomAttack = 1;
    }

    private IEnumerator CircleFormationRoutine(Vector3 center, float radius, float startAngle, float moveDuration, float rotationSpeed, float spawnDelay)
    {
        randomAttack = 0;
        if (spawnDelay > 0f)
        {
            SetPathDelayVisible(false);
            yield return new WaitForSeconds(spawnDelay);
        }

        SetPathDelayVisible(true);
        float radians = startAngle * Mathf.Deg2Rad;
        Vector3 circlePosition = center + new Vector3(Mathf.Cos(radians) * radius, Mathf.Sin(radians) * radius, 0f);
        circlePosition = ClampToVisibleWidth(circlePosition);
        Tween moveTween = transformH.DOMove(circlePosition, moveDuration).SetEase(Ease.Linear);
        yield return moveTween.WaitForCompletion();

        randomAttack = 1;
        float angle = startAngle;
        while (!GameState.won)
        {
            angle += rotationSpeed * Time.deltaTime;
            float currentRadians = angle * Mathf.Deg2Rad;
            Vector3 orbitPosition = center + new Vector3(Mathf.Cos(currentRadians) * radius, Mathf.Sin(currentRadians) * radius, 0f);
            transformH.position = ClampToVisibleWidth(orbitPosition);
            yield return null;
        }
    }

    private static Vector3 ClampToVisibleWidth(Vector3 position)
    {
        position.x = ClampToVisibleWidth(position.x);
        return position;
    }

    private static float ClampToVisibleWidth(float x)
    {
        Camera camera = Camera.main;
        if (camera == null || !camera.orthographic)
            return Mathf.Clamp(x, -1.8f, 1.8f);

        const float horizontalPadding = 0.35f;
        float halfWidth = camera.orthographicSize * camera.aspect;
        float visibleLimit = Mathf.Max(0.45f, halfWidth - horizontalPadding);
        return Mathf.Clamp(x, -visibleLimit, visibleLimit);
    }

    public void SetPathDelayVisible(bool visible)
    {
        hideForPathDelay = !visible;
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = visible;
        }

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>(true);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = visible;
        }
    }

    IEnumerator HideEnemy()
    {
        waitDontShot = moveObj.timeDelay + moveObj.timeRun + 2;
        yield return new WaitForSeconds(waitDontShot);
        randomAttack = 0;
        yield return wait6;
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        if (idEnemy == 10)
        {
            SpawnBulletWhenDie();
        }
        if (WaveManager.instance != null)
        {
            WaveManager.instance.countEnemy -= 1;
        }
        transform.DOKill();
        StopAllCoroutines();
        if (GameManager.isDestroyAll)
        {
            ExplosionControl explosion = GameManager.Instance.explosionFix1Pool.New();
            explosion.SetInfo(transform.position);
        }
        if (GameManager.Instance != null)
            GameManager.Instance.enemyPool.Store(this);
        //StopAllCoroutines();
    }

    public void SetInfo(EnemyType enemyType, float difficulty)
    {
        EnsureSpriteRenderer();
        EnemyConfigData config = GameConfigService.Instance.GetEnemy(enemyType);
        idEnemy = (int)enemyType;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = GetConfiguredSprite(enemyType, config);

            spriteRenderer.enabled = true;
            spriteRenderer.color = colorCurrent;
            EnsureVisibleRenderer();
        }
        heath = (int)(config.health * difficulty);
        currentHP = heath;
        score = (int)(config.score * difficulty);
        atk = (int)(config.attack * difficulty);
        def = (int)(config.defend * difficulty);
        fireRate = config.fireRate;

        SetHPBar();
    }

    private Sprite GetConfiguredSprite(EnemyType enemyType, EnemyConfigData config)
    {
        if (config != null && config.sprite != null)
            return config.sprite;

        int spriteIndex = enemyType == EnemyType.Basic ? 0 : 1;
        if (listSprite != null && spriteIndex >= 0 && spriteIndex < listSprite.Count && listSprite[spriteIndex] != null)
            return listSprite[spriteIndex];

        Debug.LogWarning("[EnemyControl] Missing configured sprite for " + enemyType + " on " + gameObject.name);
        return spriteRenderer != null ? spriteRenderer.sprite : null;
    }

    public void ConfigureCombat(float attackScale, float fireRateMultiplier, bool canShoot)
    {
        atk = Mathf.Max(1, Mathf.RoundToInt(atk * Mathf.Max(0.01f, attackScale)));
        fireRate *= Mathf.Max(0.05f, fireRateMultiplier);
        this.canShoot = canShoot;
    }

    private void EnsureSpriteRenderer()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        }
    }

    private void EnsureVisibleRenderer()
    {
        if (hideForPathDelay)
        {
            return;
        }

        EnsureSpriteRenderer();
        if (spriteRenderer == null)
        {
            return;
        }

        if (spriteRenderer.sprite == null && listSprite != null && listSprite.Count > 0)
        {
            int spriteIndex = idEnemy == (int)EnemyType.Heavy ? 1 : 0;
            spriteIndex = Mathf.Clamp(spriteIndex, 0, listSprite.Count - 1);
            spriteRenderer.sprite = listSprite[spriteIndex];
        }

        spriteRenderer.enabled = true;
        if (spriteRenderer.sortingOrder < VisibleSortingOrder)
        {
            spriteRenderer.sortingOrder = VisibleSortingOrder;
        }
    }

    public void SetHPBar()
    {
        if (!hpBar.activeInHierarchy && currentHP < heath)
        {
            hpBar.SetActive(true);
        }
        hpScale.Set((float)currentHP / (float)heath, hpTrans.localScale.y, 0);
        hpTrans.localScale = hpScale;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(ListTag.TAG_DANTA))  // trung dan tu may bay chinh
        {
            if (def == MyPlaneController.instance.att)
                currentHP -= 30;
            else if (def > MyPlaneController.instance.att)
                currentHP -= 20;
            else if (def < MyPlaneController.instance.att)
            {
                if (MyPlaneController.instance.att - def > 10)
                    currentHP -= (MyPlaneController.instance.att - def);
                else
                    currentHP -= 30;
            }
            SetHPBar();
            other.gameObject.SetActive(false);
            spriteRenderer.color = colorTakeDmg;
            StartCoroutine(TurnOffAnim());
        }
        if (other.gameObject.CompareTag(ListTag.TAG_DANTA2))  // trung dan tu may bay chinh
        {
            currentHP -= (int)(GameState.dmgSupport / 3);
            SetHPBar();
            spriteRenderer.color = colorTakeDmg;
            StartCoroutine(TurnOffAnim());
        }
        if (other.gameObject.CompareTag(ListTag.TAG_PLAYER) || other.gameObject.CompareTag(ListTag.TAG_DAN6) || other.gameObject.CompareTag(ListTag.TAG_SHIELD))
        {
            SetHPBar();
            currentHP = 0;
        }
        if (other.gameObject.CompareTag(ListTag.TAG_DAN5))
        {
            currentHP -= GameState.dmgRocket;
            ExplosionControl explosion = GameManager.Instance.explosionFix2Pool.New();
            explosion.SetInfo(transform.position);
            SetHPBar();
            other.gameObject.SetActive(false);
        }

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(ListTag.TAG_DAN3))
        {
            if (!onStayDmg)
            {
                StartCoroutine(StayDamage(60));
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag(ListTag.TAG_BULLET9))
        {
            if (!onStayDmg)
            {
                if (def == MyPlaneController.instance.att)
                    currentHP -= 100;
                else if (def > MyPlaneController.instance.att)
                    currentHP -= 70;
                else if (def < MyPlaneController.instance.att)
                {
                    if (MyPlaneController.instance.att - def > 10)
                        currentHP -= (MyPlaneController.instance.att - def) * 3;
                    else
                        currentHP -= 70;
                }
                StartCoroutine(StayDamage(0));
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag(ListTag.TAG_DANTA26))
        {
            if (!onStayDmg)
            {
                currentHP -= 50;
                StartCoroutine(StayDamage(0));
                onStayDmg = true;
            }
        }
    }
    IEnumerator StayDamage(int dmg)
    {
        currentHP -= dmg;
        SetHPBar();
        ExplosionControl explosion = GameManager.Instance.explosionE01Pool.New();
        explosion.SetInfo(transform.position);
        spriteRenderer.color = colorTakeDmg;
        StartCoroutine(TurnOffAnim());
        yield return waitStayDmg;
        onStayDmg = false;
    }
    // return ve mau ban dau
    IEnumerator TurnOffAnim()
    {
        yield return waitReturnColor;
        spriteRenderer.color = colorCurrent;
    }

    private void DropItem()
    {
        int itemIndex = EnemyDropRules.GetConfiguredItemIndex(idItemDrop);
        if (itemIndex < 0)
            return;

        Itemcontroller newItem = GameManager.Instance.itemPool.New();
        newItem.SetInfo(itemIndex, transform.position);
    }
    private void DropItemRandom()
    {
        if (Random.Range(0, 160) >= 4)
            return;

        int itemIndex = EnemyDropRules.GetRandomItemIndex(Random.Range(0, 40));
        if (itemIndex < 0)
            return;

        Itemcontroller newItem = GameManager.Instance.itemPool.New();
        newItem.SetInfo(itemIndex, transform.position);

    }
    //void LateUpdate()
    //{

    //   
    //}
    /// <summary>
    /// show score vs text if combo
    /// </summary>
    void ShowText()
    {
        textPos.Set(transform.position.x, transform.position.y + 0.5f, 0);
        if (score > 0)
        {
            ShowScoreText scoreText = GameManager.Instance.scoreAddedPool.New();
            scoreText.SetInfo(score, textPos);
        }
        #region hien thi text khen nguoi choi
        if (transform.position.x > 2 || transform.position.x < -2)
        {
            showPos = transform.position;
        }
        else
        {
            showPos = transform.position;
        }
        TestEffText newNoti;// = GameManager.Instance.NotiPool.New();
        if (GameState.hit == 10)
        {
            newNoti = GameManager.Instance.NotiPool.New();
            newNoti.SetInfo(GameManager.Instance.NotiTextList[0], showPos);
        }
        if (GameState.hit == 20)
        {
            newNoti = GameManager.Instance.NotiPool.New();
            newNoti.SetInfo(GameManager.Instance.NotiTextList[1], showPos);

        }
        if (GameState.hit == 30)
        {
            newNoti = GameManager.Instance.NotiPool.New();
            newNoti.SetInfo(GameManager.Instance.NotiTextList[2], showPos);

        }
        if (GameState.hit == 40)
        {
            newNoti = GameManager.Instance.NotiPool.New();
            newNoti.SetInfo(GameManager.Instance.NotiTextList[3], showPos);

        }
        if (GameState.hit == 60)
        {
            newNoti = GameManager.Instance.NotiPool.New();
            newNoti.SetInfo(GameManager.Instance.NotiTextList[4], showPos);

        }
        #endregion

    }

    #region Damage by Darkbuster Skill
    IEnumerator StayDamageDarkBuster(int dmg)
    {
        if (transformH.position.y < 4f)
        {
            currentHP -= dmg;
            SetHPBar();
            ExplosionControl explosion = GameManager.Instance.explosionE01Pool.New();
            explosion.SetInfo(transform.position);
            spriteRenderer.color = colorTakeDmg;
            StartCoroutine(TurnOffAnim());
        }
        yield return new WaitForSeconds(0.5f);
        onStayDmg = false;
    }

    public void ActiveDmgByDarkBuster()
    {
        onStayDmg = false;
    }
    #endregion

    void Update()
    {
        if (GameState.isGamePaused == false)
        {
            EnsureVisibleRenderer();
            TryStartScreenRetreat();
            if (GameManager.isDestroyAll)
            {
                GameState.IncreaseHit();
                gameObject.SetActive(false);
            }
            if (movementMode == EnemyMovementMode.StraightDown)
            {
                if (!isScreenRetreating)
                {
                    transformH.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
                }

                if (transformH.position.y < -5f)
                {
                    gameObject.SetActive(false);
                }
            }
            #region NPC ban dan
            if (GameState.isGamePaused == false)
            {
                if (transformH.position.y < 3.8f && transformH.position.y > -3.8f && transformH.position.x > -2f && transformH.position.x < 2f)
                {
                    if (Time.time > nextFire && randomAttack == 1 && canShoot)
                    {
                        nextFire = Time.time + fireRate;

                        if (ObjectPoolerManager.Instance != null && ObjectPoolerManager.Instance.DanDichPooler != null)
                        {
                            GameObject dandich = ObjectPoolerManager.Instance.DanDichPooler.GetPooledObject();
                            if (dandich != null)
                            {
                                EnemyProjectile bullet = dandich.GetComponent<EnemyProjectile>();
                                if (bullet != null)
                                {
                                    bullet.damage = atk;
                                }
                                dandich.transform.position = transform.position;
                                dandich.SetActive(true);
                            }
                        }
                    }
                }
            }
            #endregion

            if (currentHP <= 0)
            {
                if (idEnemy != 10)
                {
                    ExplosionControl explosion = GameManager.Instance.explosionFix1Pool.New();
                    explosion.SetInfo(transform.position);
                }
                else
                {
                    ExplosionControl explosion = GameManager.Instance.explosionFix2Pool.New();
                    explosion.SetInfo(transform.position);
                    CamiAni.instance.StartShake();
                }
                for (int i = 0; i < rand; i++)
                {
                    CoinEffControl gold = GameManager.Instance.goldPool.New();
                    gold.SetInfo(transform.position);
                }
                DropItemRandom();
                if (GameState.currentLevel <= 5)
                {
                    DropItem();
                }
                GameState.AddScore(score);
                GameState.IncreaseHit();
                idItemDrop = 0;
                ShowText();
                transform.DOKill();
                gameObject.SetActive(false);
            }
        }
    }

    private void TryStartScreenRetreat()
    {
        if (hasScreenRetreated || isScreenRetreating || hideForPathDelay || transformH == null)
        {
            return;
        }

        if (transformH.position.y <= GetRetreatTriggerY())
        {
            StartCoroutine(ScreenRetreat());
        }
    }

    private float GetRetreatTriggerY()
    {
        Camera cam = Camera.main;
        if (cam == null || !cam.orthographic)
        {
            return -1.5f;
        }

        float top = cam.transform.position.y + cam.orthographicSize;
        float bottom = cam.transform.position.y - cam.orthographicSize;
        return Mathf.Lerp(top, bottom, 0.55f);
    }

    private IEnumerator ScreenRetreat()
    {
        hasScreenRetreated = true;
        isScreenRetreating = true;
        int previousAttackState = randomAttack;
        randomAttack = 0;

        if (movementMode == EnemyMovementMode.FollowPath)
        {
            DOTween.Pause(transform);
        }

        Vector3 returnPosition = transformH.position;
        Vector3 retreatPosition = returnPosition + Vector3.up * 1.35f;
        retreatPosition.y = Mathf.Min(retreatPosition.y, 3.9f);

        yield return transformH.DOMove(retreatPosition, 0.75f)
            .SetEase(Ease.OutSine)
            .WaitForCompletion();
        yield return new WaitForSeconds(0.15f);
        yield return transformH.DOMove(returnPosition, 0.65f)
            .SetEase(Ease.InSine)
            .WaitForCompletion();

        if (movementMode == EnemyMovementMode.FollowPath)
        {
            DOTween.Play(transform);
        }

        randomAttack = previousAttackState;
        isScreenRetreating = false;
    }

    private void SpawnBulletWhenDie()
    {
        BulletPatternController.Instance.FireEnemyDeathBurst(transform.position, atk);
    }
}
