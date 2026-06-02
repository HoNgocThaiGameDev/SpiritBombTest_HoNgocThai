using UnityEngine;
using TMPro;
using System.Collections;
using System.Net;
using UnityEngine.UI;
using ProgressBar;
using UnityEngine.EventSystems;
using DG.Tweening;

// gioi han vung di chuyen cua thang may bay
[System.Serializable]
public class Boundary
{
    public GameObject myCamera;
    public float xMin, xMax;
    public float yMin, yMax;

    // tinh duoc gioi han cua y
    void ClampAxis()
    {
        xMin = -2f;
        xMax = 2f;
        yMin = myCamera.transform.position.y - 3f;
        yMax = myCamera.transform.position.y + 2.4f;
    }
}

[RequireComponent(typeof(AudioSource))]
public class MyPlaneController : MonoBehaviour
{
    public static MyPlaneController instance;
    //Spine
    public Camera followingCam;
    // quan ly am anh trong game
    AudioSource myaudio;
    public GameObject soundStart;
    public AudioClip getCoin;
    public AudioClip getItem;
    public bool isLose;
    public bool fireRocket;
    public Boundary boundary;
    public GameObject myCamera;     // main camera
    public GameObject saveMePanel;  // pop up save me khi game over
    public GameObject[] planeSkeleton;
    // toa do cac nong sung
    public Transform[] fireBullet;
    public Transform fireRocketPos; // vi tri ban cua rocket
    public Transform fireRocketPos2; // vi tri ban cua rocket

    public bool isMoveEnd;
    public bool isSupport6Att;
    public GameObject shield;           // giap 
    public GameObject shieldBar;        // giap con bao nhieu % 
    public GameObject heathbBar;        // so luong mau may bay chinh
    public GameObject laser;
    public TMP_Text textHealth;
    public GameObject effectHPLost;

    public RectTransform buttonIndicatorRight;
    public RectTransform buttonIndicatorLeft;
    Vector3 offset2Camera;          // do lech ban dau vs main camera
    Vector3 offset;                 // do lech dau ngon tay vs gameobject
    Vector3 vec;
    Vector3 lastPos;
    private Vector3 touchPos;
    // thong so may bay
    float nextFire = 0;
    float fireRate = 0.12f;
    public float subFireRate = 0.18f;
    public float speedAttack = 0;
    float offsetLevel = 0.01f;

    float nextFireRocket = 0;
    float rocketRate = 0.7f;
    [HideInInspector]
    public string dan;         // loai dan dang ban
    Vector3 mousePos;
    public int level;
    private int bulletBoostLevel;
    private int laserBoostLevel;
    private Coroutine bulletBoostCoroutine;
    private Coroutine laserBoostCoroutine;
    bool isFirstMove;
    public int att;
    public int def;
    public float speed;
    public float energy;
    public int numberMissile = 0;
    public TMP_Text txtTotalEnergy;
    public float totalEnergy;
    public float totalShield;
    public Transform currentPos;
    float mount;
    public bool onStayDmg;
    public bool isFire;
    public GameObject crackView;
    bool isShowCrack;
    private Vector3 screenPos;
    private Vector3 newCamPos;
    private Animator healthBarAnimator;
    private Image healthBarImage;
    private Slider healthBarSlider;
    private Slider shieldBarSlider;
    public Image shieldBarImage;
    [SerializeField] private float statusBarTweenDuration = 0.25f;
    [SerializeField] private Ease statusBarTweenEase = Ease.OutQuad;
    private Tween healthBarTween;
    private Tween shieldBarTween;
    private Tween healthBarImageTween;
    private Tween shieldBarImageTween;
    [SerializeField] private float spawnDamageDelay = 4f;
    private const long ProjectileHitVibrationMs = 45;
    private const int ProjectileHitVibrationAmplitude = 90;
    private const long CollisionHitVibrationMs = 90;
    private const int CollisionHitVibrationAmplitude = 170;
    private const long HeavyHitVibrationMs = 130;
    private const int HeavyHitVibrationAmplitude = 230;
    private float damageEnabledTime;
    public CircleCollider2D colliderPlayer;
    private Vector3 posFirstMove;
    //
    private WaitForSeconds waitReturnRocket;
    private WaitForSeconds waitReturnShield;
    private WaitForSeconds waitReturnLaser;
    private WaitForSeconds waitReturnSpeed;
    private WaitForSeconds waitStayDmg;
    private WaitForSeconds waitDestroyAll;

    private float indexDmg;
    private float hp1_2 = 0;
    private float hp1_4 = 0;
    private int valueX2Gold;
    private Vector3 startPos;
    void Awake()
    {
        instance = this;

        int levelMissile = GameState.levelMissile < 6 ? GameState.levelMissile : 5;

        numberMissile = GameConfigService.Instance.GetMissileUpgrade(levelMissile).number;
        waitReturnRocket = new WaitForSeconds(10);
        waitReturnShield = new WaitForSeconds(GameConfigService.Instance.GetShieldUpgrade(GameState.levelShield).duration);
        waitReturnLaser = new WaitForSeconds(20);
        waitReturnSpeed = new WaitForSeconds(15);
        waitStayDmg = new WaitForSeconds(0.2f);
        waitDestroyAll = new WaitForSeconds(1);
    }
    void Start()
    {
        instance = this;
        valueX2Gold = 1;
        onStayDmg = false;
        isSupport6Att = false;
        GameState.isMoveMap = true;
        newCamPos = followingCam.transform.position;
        isShowCrack = true;
        CheckCurrentPlane();
        isFire = false;
        isMoveEnd = false;
        damageEnabledTime = float.PositiveInfinity;
        dan = "Dan1";
        att = GameState.attackPlane;
        def = GameState.defendPlane;
        speed = GameState.speedAttPlane;

        energy = GameState.energyPlane;
        totalEnergy = energy; // get total energy
        txtTotalEnergy.text = totalEnergy.ToString();
        hp1_2 = totalEnergy / 2;
        hp1_4 = totalEnergy / 4;
        isLose = false;
        offset2Camera = new Vector3(0, 3f, -10f);
        offset = new Vector3(0, 0.5f, 0);
        lastPos = Vector3.zero;
        myaudio = GetComponent<AudioSource>();
        if (textHealth != null)
        {
            textHealth.gameObject.SetActive(true);
            textHealth.text = energy.ToString();
        }
        level = GameState.GetLevelPlane(0);
        CheckBullet();
        if (heathbBar != null)
        {
            heathbBar.SetActive(true);
            healthBarAnimator = heathbBar.GetComponent<Animator>();
            healthBarSlider = heathbBar.GetComponent<Slider>();
            healthBarImage = GetFillImage(heathbBar);
            SetHealthBarValue(1f, false);
        }
        if (shieldBar != null)
        {
            shieldBarSlider = shieldBar.GetComponent<Slider>();
            shieldBarImage = GetFillImage(shieldBar);
            SetShieldBarValue(0f, false);
        }
        offsetLevel = (float)0.016 * (level - 1);
        posFirstMove.Set(0, 0.04f, 0);
        startPos.Set(0, -7f, 1);
        StartCoroutine(WaitFew());

        if (GameState.currentLevel > 1 && GameState.currentLevel <= 10)
        {
            indexDmg = 0;
        }
        if (GameState.currentLevel > 10 && GameState.currentLevel <= 20)
        {
            indexDmg = 5;
        }
        if (GameState.currentLevel > 20 && GameState.currentLevel <= 30)
        {
            indexDmg = 10;
        }
        if (GameState.currentLevel > 30 && GameState.currentLevel <= 40)
        {
            indexDmg = 15;
        }
        if (GameState.currentLevel > 40 && GameState.currentLevel <= 50)
        {
            indexDmg = 15;
        }
        if (GameState.currentLevel > 50 && GameState.currentLevel <= 60)
        {
            indexDmg = 20;
        }
    }

    private bool IsSkillShieldActive()
    {
        return false;
    }

    IEnumerator WaitFew()
    {
        yield return new WaitForSeconds(1);
        soundStart.SetActive(true);
    }
    //check type bullet
public void CheckBullet()
    {
        GameState.currentPlane.Value = 1;
        if (laserBoostLevel > 0)
        {
            dan = "Dan6";
        }
        else if (bulletBoostLevel > 0)
        {
            dan = "Dan3";
        }
        else
        {
            dan = "Bullet1";
        }
        GameState.currentFireRate.Value = fireRate = Mathf.Max(0.05f, 0.2f - (float)level / 100f);
        GameState.currentSubFireRate.Value = subFireRate = fireRate;
        GameState.currentSpeedAtt.Value = speedAttack = 7f + level / 10f + speed;
    }
void CheckCurrentPlane()
    {
        GameState.currentPlane.Value = 1;

        if (planeSkeleton == null)
        {
            return;
        }

        for (int i = 0; i < planeSkeleton.Length; i++)
        {
            if (planeSkeleton[i] != null)
            {
                planeSkeleton[i].SetActive(i == 0);
            }
        }

        PlayerPlaneSpriteApplicator.ApplyToHierarchy(
            planeSkeleton[0],
            GameConfigService.Instance.GetPlayerPlane());
    }

    private Vector3 previousPos;
    private bool shouldMove;
    private readonly string TagList;

    void Update()
    {
        //UpdateProgressbar();
            if (GameState.isGamePaused == false && GameState.won == false)
            {
                FireRocket();
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    #region dieu khien may bay bang tay kieu 2: theo sat dau ngon tay
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);

                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                //Debug.Log("TouchPhase Began");
                                //if (!IsTouchOnButtonErea(Camera.main.ScreenToWorldPoint(touch.position)))
                                previousPos = Camera.main.ScreenToWorldPoint(touch.position);

                                break;
                            case TouchPhase.Moved:
                                // delta dich chuyen
                                //Debug.Log("TouchPhase Moved");
                                shouldMove = true;
                                Vector3 deltaPos = Camera.main.ScreenToWorldPoint(touch.position) - previousPos;
                                touchPos = transform.position + deltaPos;
                                if (shouldMove)
                                {
                                    touchPos.z = 0;
                                    touchPos.x = Mathf.Clamp(touchPos.x, boundary.xMin, boundary.xMax);
                                    touchPos.y = Mathf.Clamp(touchPos.y, myCamera.transform.position.y - 3.8f, myCamera.transform.position.y + 3.8f);
                                    previousPos = Camera.main.ScreenToWorldPoint(touch.position);
                                    // so cang to cang delay
                                    transform.position = Vector3.SmoothDamp(transform.position, touchPos, ref vec, 0);
                                }

                                break;

                            case TouchPhase.Stationary:
                                //Debug.Log("TouchPhase Stationary");
                                previousPos = Camera.main.ScreenToWorldPoint(touch.position);
                                Vector3 stationaryDeltaPos = Camera.main.ScreenToWorldPoint(touch.position) - previousPos;
                                touchPos = transform.position + stationaryDeltaPos * 2.2f;
                                touchPos.z = 0;
                                touchPos.x = Mathf.Clamp(touchPos.x, boundary.xMin, boundary.xMax);
                                touchPos.y = Mathf.Clamp(touchPos.y, myCamera.transform.position.y - 3.8f, myCamera.transform.position.y + 3.8f);
                                previousPos = Camera.main.ScreenToWorldPoint(touch.position);
                                // so cang to cang delay
                                transform.position = Vector3.SmoothDamp(transform.position, touchPos, ref vec, 0.01f);
                                break;


                        }
                    }
                }
                #endregion
                #region dieu khien may bay bang chuot trong editor
#if UNITY_EDITOR
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    if (Input.GetMouseButton(0))
                    {

                        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePos.z = 0;
                        //transform.position = new Vector3(mousePos.x, mousePos.y + 0.5f, 0);
                        if ((transform.position - mousePos).magnitude > 0.5f)
                        {
                            transform.position = Vector3.SmoothDamp(transform.position, mousePos, ref vec, 0.15f);
                        }
                        else
                            //Vector3.Lerp(transform.position, mousePos, 1 - Mathf.Exp(-20 * Time.deltaTime));
                            //else if ((transform.position - mousePos).magnitude > 0.1f)
                            //    transform.position = Vector3.SmoothDamp(transform.position, mousePos, ref vec, 0.05f);
                            //else
                            transform.position = Vector3.SmoothDamp(transform.position, mousePos, ref vec, 0.01f);
                        IsTouchOnButtonErea(Input.mousePosition);
                    }
                }
#endif
                #endregion

            }
            //First Move player
            if (!isFirstMove)
            {
                transform.position += posFirstMove;
                if (transform.position.y >= -2.5f)
                {
                    isFire = true;
                    isFirstMove = true;
                    ResetSpawnProtection();
                }
            }
            if (isFirstMove && !GameState.won && isFire)
                Fire(); //fire bullet 

            //if win stage
            if (GameState.won && isMoveEnd)
            {
                transform.position += new Vector3(0, 0.1f, 0);
            }
            if (energy <= 0)
            {
                fireRocket = false;
                if (laser != null)
                    laser.SetActive(false);
                if (CamiAni.instance != null)
                    CamiAni.instance.StartShake();
                //GameState.isMoveMap = false;
                if (saveMePanel != null)
                    saveMePanel.SetActive(true);
                if (GameManager.Instance != null && GameManager.Instance.explosionFix1Pool != null)
                {
                    ExplosionControl explosion = GameManager.Instance.explosionFix1Pool.New();
                    explosion.SetInfo(transform.position);
                }
                transform.position = startPos;
                isFire = false;
                isFirstMove = false;
                gameObject.SetActive(false);
                GameState.isGamePaused = true;
            }
    }
    private bool IsTouchOnButtonErea(Vector3 touchPos)
    {
        bool result = false;
        Vector3 touchPosOnScreen = Camera.main.WorldToScreenPoint(touchPos);
        //Debug.Log("IsTouchOnButtonErea touchPos " + touchPosOnScreen.ToString());
        Vector2 restrictScreenPoint;
        //Debug.Log("IsTouchOnButtonErea restrictScreenPoint " + restrictScreenPoint.ToString());
        int type = PlayerSettingsService.GetControl();
        if (type == 2)
        {
            restrictScreenPoint = Camera.main.WorldToScreenPoint(buttonIndicatorRight.position);
            if (touchPosOnScreen.x > restrictScreenPoint.x && touchPosOnScreen.y < restrictScreenPoint.y)
            {
                result = true;
            }
        }
        else
        {
            restrictScreenPoint = Camera.main.WorldToScreenPoint(buttonIndicatorLeft.position);
            if (touchPosOnScreen.x < restrictScreenPoint.x && touchPosOnScreen.y < restrictScreenPoint.y)
            {
                result = true;
            }
        }
        //Debug.Log("IsTouchOnButtonErea result " + result.ToString());

        return result;
    }
    void OnEnable()
    {
        instance = this;
        onStayDmg = false;
    }

    private void OnDisable()
    {
        KillStatusBarTweens();
    }

    private void KillStatusBarTweens()
    {
        healthBarTween.Kill();
        shieldBarTween.Kill();
        healthBarImageTween.Kill();
        shieldBarImageTween.Kill();
        healthBarTween = null;
        shieldBarTween = null;
        healthBarImageTween = null;
        shieldBarImageTween = null;
    }

    void OnReduceHP()
    {
        ExplosionControl explosion = GameManager.Instance.explosionE01Pool.New();
        explosion.SetInfo(transform.position);
        if (!effectHPLost.activeInHierarchy)
        {
            effectHPLost.SetActive(true);
        }
        if (PlayerSettingsService.IsVibrationEnabled())
            Vibration.Vibrate(ProjectileHitVibrationMs, ProjectileHitVibrationAmplitude);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!CanReceiveDamage() && IsDamageCollider(other))
        {
            DisableIncomingProjectile(other);
            return;
        }

        if (other.CompareTag(ListTag.TAG_DANDICH) && shield.activeInHierarchy == false)
        {
            //-HP
            if (!IsSkillShieldActive())
            {
                EnemyProjectile enemyBullet = other.GetComponent<EnemyProjectile>();
                int bulletDamage = enemyBullet != null
                    ? enemyBullet.damage
                    : (EnemyProjectile.instance != null ? EnemyProjectile.instance.damage : 0);
                mount = PlayerDamageRules.GetEnemyProjectileDamage(bulletDamage, def, indexDmg);
                energy -= mount;
                UpdateEnergy(mount);
                OnReduceHP();
                other.gameObject.SetActive(false);
            }
        }
        if (other.CompareTag(ListTag.TAG_DANDICH2) && shield.activeInHierarchy == false)
        {
            if (!IsSkillShieldActive())
            {
                mount = PlayerDamageRules.GetHeavyProjectileDamage(def, totalEnergy, indexDmg);
                energy -= mount;
                UpdateEnergy(mount);
                OnReduceHP();
                other.gameObject.SetActive(false);
            }
        }
        if (other.CompareTag(ListTag.TAG_BIGROCKET) )
        {
            if (!IsSkillShieldActive())
            {
                if (shield.activeInHierarchy)
                {
                    shield.SetActive(false);
                }
                else
                {
                    mount = energy;
                    energy -= mount;
                    UpdateEnergy(mount);
                }
                ExplosionControl explosion = GameManager.Instance.explosionFix2Pool.New();
                explosion.SetInfo(other.transform.position);
                other.gameObject.SetActive(false);
            }
        }
        if (other.CompareTag(ListTag.TAG_ENEMY) && shield.activeInHierarchy == false)
        {
            //StartCoroutine(CritHit());
            if (!IsSkillShieldActive())
            {
                mount = PlayerDamageRules.GetEnemyCollisionDamage(indexDmg);
                energy -= mount;
                UpdateEnergy(mount);
                if (!effectHPLost.activeInHierarchy)
                    effectHPLost.SetActive(true);
                if (PlayerSettingsService.IsVibrationEnabled())
                    Vibration.Vibrate(CollisionHitVibrationMs, CollisionHitVibrationAmplitude);
            }
        }

        if (other.CompareTag(ListTag.TAG_ITEM3))
        {
            myaudio.PlayOneShot(getItem, 1F);

            ItemCollisEffect effect = GameManager.Instance.itemCollisonPool.New();
            effect.SetInfo(transform.position);

            ActivateBulletBoost(5);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag(ListTag.TAG_ITEM5))
        {
            GameState.useRocket.Value += 1;
            if (fireRocket == true)
            {
                myaudio.PlayOneShot(getItem, 1F);
                other.gameObject.SetActive(false);
            }
            else
            {
                fireRocket = true;
                myaudio.PlayOneShot(getItem, 1F);
                StartCoroutine(ReturnBullet3());
                other.gameObject.SetActive(false);
            }
            ItemCollisEffect effect = GameManager.Instance.itemCollisonPool.New();
            effect.SetInfo(transform.position);
            AdjustFireRate();
        }
        if (other.CompareTag(ListTag.TAG_ITEM6))
        {
            myaudio.PlayOneShot(getItem, 1F);
            ActivateLaserBoost(6);
            ItemCollisEffect effect = GameManager.Instance.itemCollisonPool.New();
            effect.SetInfo(transform.position);

            other.gameObject.SetActive(false);

        }

        // crystal
        if (other.gameObject.CompareTag(ListTag.TAG_ITEMCRYSTAL))
        {
            myaudio.PlayOneShot(getItem, 1F);
            TestEffText newNoti = GameManager.Instance.NotiPool.New();
            newNoti.SetInfo(GameManager.Instance.NotiTextList[0], transform.position);
            GameState.AddCrystal(1);
            GameState.currentCrystal.Value++;
            GameState.AddScore(10);
            ItemCollisEffect effect = GameManager.Instance.itemCollisonPool.New();
            effect.SetInfo(transform.position);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag(ListTag.TAG_ITEMGOLD))
        {
            other.gameObject.SetActive(false);
            myaudio.PlayOneShot(getCoin, 0.7F);

            if (GameState.isX2Coin)
            {
                SubCoin(other, 2);
            }
            else
            {
                SubCoin(other, 1);
            }
        }

        // shield
        if (other.gameObject.CompareTag(ListTag.TAG_ITEMSHIELD))
        {
            GameState.useShield.Value += 1;
            myaudio.PlayOneShot(getItem, 1F);
            ItemCollisEffect effect = GameManager.Instance.itemCollisonPool.New();
            effect.SetInfo(transform.position);
            GamePlayEventListener.instance.SetValueShield();
            totalShield = GameState.shield; //get totalShield
            shield.SetActive(true);
            other.gameObject.SetActive(false);
            StartCoroutine(HideShield());
        }
        // speed att
        if (other.gameObject.CompareTag(ListTag.TAG_ITEMSPEED))
        {
            myaudio.PlayOneShot(getItem, 1F);
            ItemCollisEffect effect = GameManager.Instance.itemCollisonPool.New();
            effect.SetInfo(transform.position);
            //increase level + 1
            if (level <= 7)
            {
                level += 1;
            }
            StartCoroutine(ReturnSpeedAtt());
            speedAttack = speedAttack * 1.3f;
            fireRate /= 1.5f;
            subFireRate /= 1.5f;
            other.gameObject.SetActive(false);
        }
        // speed att
        if (other.gameObject.CompareTag(ListTag.TAG_ITEMDESTROY))
        {
            CamiAni.instance.StartShake();
            GameState.useNukeBomb.Value += 1;
            myaudio.PlayOneShot(getItem, 1F);
            ItemCollisEffect effect = GameManager.Instance.itemCollisonPool.New();
            effect.SetInfo(transform.position);
            ExplosionControl explosion = GameManager.Instance.explosion_AllPool.New();
            explosion.SetInfo(transform.position);
            GameManager.isDestroyAll = true;
            StartCoroutine(WaitDestroyAll());

            if (WaveManager.instance != null)
            {
                WaveManager.instance.timeNextWave = 0;
            }

            other.gameObject.SetActive(false);
        }
        // an item mau
        if (other.gameObject.CompareTag(ListTag.TAG_ITEMENERGY))
        {
            myaudio.PlayOneShot(getItem, 1F);
            ItemCollisEffect effect = GameManager.Instance.itemCollisonPool.New();
            effect.SetInfo(transform.position);
            if (energy < totalEnergy)
            {
                energy += totalEnergy * 0.25f;
                if (energy >= totalEnergy)
                    energy = totalEnergy;
                UpdateEnergy(-(totalEnergy * 0.25f));
            }
            other.gameObject.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!CanReceiveDamage() && IsDamageCollider(other))
        {
            return;
        }

        if (other.gameObject.CompareTag(ListTag.TAG_DANDICH3))
        {
            if (!onStayDmg && shield.activeInHierarchy == false)
            {
                StartCoroutine(StayDamage(30));
                onStayDmg = true;
            }
            else if (!onStayDmg && shield.activeInHierarchy == true)
            {
                shield.SetActive(false);
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag(ListTag.TAG_LASER))
        {
            if (!onStayDmg)
            {
                StartCoroutine(StayDamage((int)(totalEnergy * 0.1f)));
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag(ListTag.TAG_BOSS1) || other.gameObject.CompareTag(ListTag.TAG_CAPTAIN))
        {
            if (!onStayDmg)
            {
                StartCoroutine(StayDamage(50));
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag(ListTag.TAG_BOSS2))
        {
            if (!onStayDmg)
            {
                StartCoroutine(StayDamage(70));
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag(ListTag.TAG_BOSS3))
        {
            if (!onStayDmg)
            {
                StartCoroutine(StayDamage(100));
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag(ListTag.TAG_BOSS4))
        {
            if (!onStayDmg)
            {
                StartCoroutine(StayDamage(150));
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag(ListTag.TAG_MINIBOSS))
        {
            if (!onStayDmg)
            {
                StartCoroutine(StayDamage(40));
                onStayDmg = true;
            }
        }
    }
    IEnumerator StayDamage(int dmg)
    {
        if (!CanReceiveDamage())
        {
            onStayDmg = false;
            yield break;
        }

        if (!IsSkillShieldActive())
        {
            energy -= dmg;
            UpdateEnergy(dmg);
            if (PlayerSettingsService.IsVibrationEnabled())
                Vibration.Vibrate(GetStayDamageVibrationMs(dmg), GetStayDamageVibrationAmplitude(dmg));
            //if (!effectHPLost.activeInHierarchy)
            effectHPLost.SetActive(true);
            yield return waitStayDmg;
            onStayDmg = false;
        }
    }
    // su dung ban dan ten lua
    void FireRocket()
    {
        if (Time.time > nextFireRocket && (saveMePanel.activeInHierarchy == false) && fireRocket == true)
        {
            ObjectPooler rocketPool = ObjectPoolerManager.Instance != null ? ObjectPoolerManager.Instance.rocket : null;
            if (rocketPool == null || fireRocketPos == null)
            {
                Debug.LogWarning("[MyPlaneController] Missing rocket pool or fire position.");
                fireRocket = false;
                return;
            }

            nextFireRocket = Time.time + rocketRate;
            GameObject bu1 = rocketPool.GetPooledObject();
            if (bu1 == null)
                return;

            bu1.transform.position = fireRocketPos.position;
            bu1.SetActive(true);
            for (int i = 1; i < numberMissile && fireRocketPos2 != null; i++)
            {
                GameObject bu2 = rocketPool.GetPooledObject();
                if (bu2 == null)
                    continue;

                bu2.transform.position = fireRocketPos2.position;
                bu2.SetActive(true);
            }
        }
    }

    private static long GetStayDamageVibrationMs(int damage)
    {
        return damage >= 100 ? HeavyHitVibrationMs : CollisionHitVibrationMs;
    }

    private static int GetStayDamageVibrationAmplitude(int damage)
    {
        return damage >= 100 ? HeavyHitVibrationAmplitude : CollisionHitVibrationAmplitude;
    }

    private bool CanReceiveDamage()
    {
        return Time.time >= damageEnabledTime && isFirstMove && !GameState.won && !GameState.lose && energy > 0;
    }

    public void ResetSpawnProtection()
    {
        damageEnabledTime = Time.time + spawnDamageDelay;
    }

    private bool IsDamageCollider(Collider2D other)
    {
        return other.CompareTag(ListTag.TAG_DANDICH)
            || other.CompareTag(ListTag.TAG_DANDICH2)
            || other.CompareTag(ListTag.TAG_DANDICH3)
            || other.CompareTag(ListTag.TAG_BIGROCKET)
            || other.CompareTag(ListTag.TAG_ENEMY)
            || other.CompareTag(ListTag.TAG_LASER)
            || other.CompareTag(ListTag.TAG_BOSS1)
            || other.CompareTag(ListTag.TAG_BOSS2)
            || other.CompareTag(ListTag.TAG_BOSS3)
            || other.CompareTag(ListTag.TAG_BOSS4)
            || other.CompareTag(ListTag.TAG_CAPTAIN)
            || other.CompareTag(ListTag.TAG_MINIBOSS);
    }

    private void DisableIncomingProjectile(Collider2D other)
    {
        if (other.CompareTag(ListTag.TAG_DANDICH)
            || other.CompareTag(ListTag.TAG_DANDICH2)
            || other.CompareTag(ListTag.TAG_DANDICH3)
            || other.CompareTag(ListTag.TAG_BIGROCKET))
        {
            other.gameObject.SetActive(false);
        }
    }

void Fire()
    {
        if (Time.time <= nextFire || saveMePanel.activeInHierarchy)
        {
            return;
        }

        nextFire = Time.time + fireRate;

        if (fireBullet == null || fireBullet.Length == 0)
        {
            return;
        }

        ObjectPoolerManager pooler = ObjectPoolerManager.Instance;
        if (pooler == null)
        {
            return;
        }

        int bulletLevel = GetEffectiveBulletLevel();
        if (bulletLevel <= 1)
        {
            SpawnPooledBullet(pooler.bullet1_1, fireBullet[0], false);
        }
        else if (bulletLevel == 2 && fireBullet.Length > 2)
        {
            SpawnPooledBullet(pooler.bullet1_1, fireBullet[1], false);
            SpawnPooledBullet(pooler.bullet1_1, fireBullet[2], false);
        }
        else if (bulletLevel <= 5 && fireBullet.Length > 6)
        {
            SpawnPooledBullet(pooler.bullet1_1, fireBullet[1], false);
            SpawnPooledBullet(pooler.bullet1_1, fireBullet[2], false);
            SpawnPooledBullet(pooler.bullet1_2, fireBullet[3], false);
            SpawnPooledBullet(pooler.bullet1_2, fireBullet[6], false);
        }
        else if (fireBullet.Length > 8)
        {
            SpawnPooledBullet(pooler.bullet1_1, fireBullet[1], true);
            SpawnPooledBullet(pooler.bullet1_1, fireBullet[2], true);
            SpawnPooledBullet(pooler.bullet1_2, fireBullet[3], true);
            SpawnPooledBullet(pooler.bullet1_2, fireBullet[4], true);
            SpawnPooledBullet(pooler.bullet1_2, fireBullet[5], true);
            SpawnPooledBullet(pooler.bullet1_2, fireBullet[6], true);
            SpawnPooledBullet(pooler.bullet1_2, fireBullet[7], true);
            SpawnPooledBullet(pooler.bullet1_2, fireBullet[8], true);
        }
    }
private void FirePhoenix()
    {
    }
private void FireDarkBuster()
    {
    }
void AdjustFireRate()
    {
        CheckBullet();
    }

    private int GetEffectiveBulletLevel()
    {
        return Mathf.Max(level, bulletBoostLevel, laserBoostLevel);
    }

    private void ActivateBulletBoost(int boostLevel)
    {
        bulletBoostLevel = Mathf.Max(bulletBoostLevel, boostLevel);
        if (bulletBoostCoroutine != null)
        {
            StopCoroutine(bulletBoostCoroutine);
        }

        bulletBoostCoroutine = StartCoroutine(ReturnBullet2());
        CheckBullet();
    }

    private void ActivateLaserBoost(int boostLevel)
    {
        laserBoostLevel = Mathf.Max(laserBoostLevel, boostLevel);
        if (laser != null)
        {
            laser.SetActive(true);
        }

        if (laserBoostCoroutine != null)
        {
            StopCoroutine(laserBoostCoroutine);
        }

        laserBoostCoroutine = StartCoroutine(ReturnBulletLaser());
        CheckBullet();
    }

    public void UpdateEnergy(float mount)
    {
        if (GameState.won)
        {
            return;
        }
        if (energy <= hp1_2 && energy > 0)
        {
            if (isShowCrack)
            {
                crackView.SetActive(true);
                StartCoroutine(ShowCrack());
                isShowCrack = false;
            }
        }

        if (textHealth != null)
            textHealth.text = ((int)energy).ToString();

        if (energy <= 0 && textHealth != null)
        {
            textHealth.text = "0";
        }
        if (energy >= totalEnergy && textHealth != null)
        {
            textHealth.text = totalEnergy.ToString();
        }
        if (totalEnergy > 0)
            SetHealthBarValue(energy / totalEnergy, true);
    }
    public void UpdateShield(float mount)
    {
        if (shield != null && shield.activeInHierarchy && totalShield > 0)
        {
            SetShieldBarValue(GameState.shield / totalShield, true);
        }
        else
        {
            SetShieldBarValue(0f, true);
        }
    }

    public void SetShieldBarValue(float normalizedValue, bool animate = true)
    {
        CacheShieldBar();
        normalizedValue = Mathf.Clamp01(normalizedValue);
        if (shieldBarSlider != null)
        {
            shieldBarTween.Kill();
            shieldBarTween = animate
                ? shieldBarSlider.DOValue(normalizedValue, statusBarTweenDuration).SetEase(statusBarTweenEase).SetUpdate(true)
                : null;
            if (!animate)
            {
                shieldBarSlider.value = normalizedValue;
            }
        }
        if (shieldBarImage != null)
        {
            shieldBarImageTween.Kill();
            if (animate)
            {
                shieldBarImageTween = shieldBarImage.DOFillAmount(normalizedValue, statusBarTweenDuration).SetEase(statusBarTweenEase).SetUpdate(true);
            }
            else
            {
                shieldBarImage.fillAmount = normalizedValue;
                shieldBarImageTween = null;
            }
        }
    }

    private void SetHealthBarValue(float normalizedValue, bool animate = true)
    {
        CacheHealthBar();
        normalizedValue = Mathf.Clamp01(normalizedValue);
        if (healthBarSlider != null)
        {
            healthBarTween.Kill();
            healthBarTween = animate
                ? healthBarSlider.DOValue(normalizedValue, statusBarTweenDuration).SetEase(statusBarTweenEase).SetUpdate(true)
                : null;
            if (!animate)
            {
                healthBarSlider.value = normalizedValue;
            }
        }
        if (healthBarImage != null)
        {
            healthBarImageTween.Kill();
            if (animate)
            {
                healthBarImageTween = healthBarImage.DOFillAmount(normalizedValue, statusBarTweenDuration).SetEase(statusBarTweenEase).SetUpdate(true);
            }
            else
            {
                healthBarImage.fillAmount = normalizedValue;
                healthBarImageTween = null;
            }
        }
    }

    private void CacheHealthBar()
    {
        if (heathbBar == null)
        {
            return;
        }

        if (healthBarSlider == null)
        {
            healthBarSlider = heathbBar.GetComponent<Slider>();
        }
        if (healthBarImage == null)
        {
            healthBarImage = GetFillImage(heathbBar);
        }
    }

    private void CacheShieldBar()
    {
        if (shieldBar == null)
        {
            return;
        }

        if (shieldBarSlider == null)
        {
            shieldBarSlider = shieldBar.GetComponent<Slider>();
        }
        if (shieldBarImage == null)
        {
            shieldBarImage = GetFillImage(shieldBar);
        }
    }

    private static Image GetFillImage(GameObject bar)
    {
        if (bar == null)
        {
            return null;
        }

        Slider slider = bar.GetComponent<Slider>();
        if (slider != null && slider.fillRect != null)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (fillImage != null && fillImage.type == Image.Type.Filled)
            {
                return fillImage;
            }
        }

        Image image = bar.GetComponent<Image>();
        if (image != null && image.type == Image.Type.Filled)
        {
            return image;
        }

        return null;
    }

    IEnumerator ReturnBulletLaser()
    {
        yield return waitReturnLaser;
        laserBoostLevel = 0;
        if (laser != null)
        {
            laser.SetActive(false);
        }
        laserBoostCoroutine = null;
        CheckBullet();
    }

    IEnumerator ReturnBullet2()
    {
        yield return new WaitForSeconds(5);
        bulletBoostLevel = 0;
        bulletBoostCoroutine = null;
        CheckBullet();
    }

    IEnumerator ReturnBullet3()
    {
        yield return waitReturnRocket;
        fireRocket = false;
    }
    IEnumerator HideShield()
    {
        yield return waitReturnShield;
        shield.SetActive(false);
        SetShieldBarValue(0f);
    }
IEnumerator ReturnSpeedAtt()
    {
        yield return waitReturnSpeed;
        if (GameState.GetLevelPlane(0) < 8 && level > 1)
        {
            level -= 1;
        }
        speedAttack = GameState.currentSpeedAtt;
        fireRate = GameState.currentFireRate;
        subFireRate = GameState.currentSubFireRate;
    }
    IEnumerator ShowCrack()
    {
        yield return waitReturnLaser;
        isShowCrack = true;
    }

    IEnumerator WaitDestroyAll()
    {
        yield return waitDestroyAll;
        GameManager.isDestroyAll = false;
    }

    private void SubCoin(Collider2D other, int ratio)
    {
        int valueCoin = 0;
        if (other.GetComponent<CoinEffControl>().size == 1)
        {
            valueCoin = ratio * 2 * valueX2Gold;
        }
        else if (other.GetComponent<CoinEffControl>().size == 2)
        {
            valueCoin = ratio * 3 * valueX2Gold;
        }
        else if (other.GetComponent<CoinEffControl>().size == 3)
        {
            valueCoin = ratio * 5 * valueX2Gold;
        }
        //Debug.Log(valueCoin);
        GameState.AddCoin(valueCoin);
        GameState.currentCoin.Value += valueCoin;
    }


private void SpawnPooledBullet(ObjectPooler pooler, Transform firePoint, bool copyRotation)
    {
        if (pooler == null || firePoint == null)
        {
            return;
        }

        GameObject bullet = pooler.GetPooledObject();
        bullet.transform.position = firePoint.position;
        if (copyRotation)
        {
            bullet.transform.localEulerAngles = firePoint.localEulerAngles;
        }
        bullet.SetActive(true);
    }
}
