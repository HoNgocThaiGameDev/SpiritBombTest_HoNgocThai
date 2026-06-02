using UnityEngine;
using System.Collections;
using ProgressBar;

public class Boss1Controller : MonoBehaviour
{
    Animator animHealthBar;
    // thay doi cach ban dan cua thang boss1
    public GameObject faceMask;     // mat na do
    public GameObject heathbar;     // thanh mau

    public GameObject fire1; // ben trai
    public GameObject fire2; // giua
    public GameObject fire3; // ben phai

    public GameObject body;
    Vector3 movement = Vector3.left * 0.02f;
    public GameObject warningBanner;      // canh bao co boss
    int heath;     // mau hien tai
    int maxHeath;  // mau toi da dung de tinh phan tram thanh mau
    int atk;         // dmg
    int def;         // so shield - ref den  than giap tren UI
    int eng;         // nang luong (mau)- ref den thanh mau tren UI
    int score;       // so diem duoc them
    int gold;        // no ra  gold
    bool firstMove;
    bool isAttack;
    float indexHealth;
    bool isCollis;
    bool onStayDmg;
    bool isLowHP;
    private const int Level3HealthMultiplier = 3;
    private const float EntryMoveSpeed = 0.84f;
    ProgressBarBehaviour hpBarProcess;
    private WaitForSeconds waitStayDmg;
    void Awake()
    {
        waitStayDmg = new WaitForSeconds(0.2f);
        GameState.delayBossAtt = 6;
        GameState.countBoss += 1;
    }

    private void OnEnable()
    {
        if (WaveManager.instance != null)
        {
            WaveManager.instance.countEnemy += 1;
        }
    }

    private void OnDisable()
    {
        if (WaveManager.instance != null)
        {
            WaveManager.instance.countEnemy -= 1;
        }
    }

    void Start()
    {
        animHealthBar = heathbar.GetComponent<Animator>();
        //set index boss
        indexHealth = 100;
        EnemyConfigData bossStat = GameConfigService.Instance.GetEnemy(EnemyType.Boss);
        ApplyConfiguredSprite(bossStat.sprite);

        maxHeath = bossStat.health;
        score = bossStat.score;
        atk = bossStat.attack;
        def = bossStat.defend;
        gold = Random.Range(30, 50);
        if (GameState.currentLevel.Value == 3)
        {
            maxHeath *= Level3HealthMultiplier;
        }
        heath = maxHeath;
        MyPlaneController.instance.fireRocket = false;
        if (GamePlayEventListener.instance != null)
            GamePlayEventListener.instance.SetSupportBoostActive(false);
        GameState.isMoveMap = false;
        //GameState.won = false;
        firstMove = false;
        warningBanner.SetActive(true);
        hpBarProcess = heathbar.GetComponent<ProgressBarBehaviour>();
        UpdateHP();
        MyPlaneController.instance.isFire = false;
    }

    private void ApplyConfiguredSprite(Sprite sprite)
    {
        if (sprite == null)
            return;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        if (spriteRenderer != null)
            spriteRenderer.sprite = sprite;
    }
    void Update()
    {
        //di chuyen khi vua xuat hien
        if (GameState.isGamePaused == false)
        {
            if (!firstMove)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    Mathf.MoveTowards(transform.position.y, 1f, EntryMoveSpeed * Time.deltaTime),
                    transform.position.z);
                if (transform.position.y <= 1f)
                {
                    GameState.isFire = true;
                    GameState.isMoveMap = true;
                    MyPlaneController.instance.isFire = true;
                    SoundController.instance.StopAllSound();
                    SoundController.instance.PlaySound(9);
                    isCollis = true;
                    BulletPatternController.Instance.StartBossPattern(transform, atk);
                    firstMove = true;
                }
            }


            // di chuyen con boss khi danh nhau
            if (GameState.isFire && !isAttack)
            {
                if (gameObject.transform.position.x > 1.5f)
                    movement = Vector3.left * 0.02f;
                else if (gameObject.transform.position.x < -1.5f)
                    movement = Vector3.right * 0.02f;
                transform.Translate(movement);
            }

            //health <25%
            if (indexHealth <= 25 && !isLowHP)
            {
                animHealthBar.Play("HealthBarBossLow");
                isLowHP = true;
            }
            if (indexHealth <= 0)
            {
                CamiAni.instance.StartShake();
                GameState.totalHit.Value += 1;
                for (int i = 0; i < gold; i++)
                {
                    CoinEffControl goldObj = GameManager.Instance.goldPool.New();
                    goldObj.SetInfo(transform.position);
                }
                ExplosionControl explosion = GameManager.Instance.explosionBossPool.New();
                explosion.SetInfo(transform.position);
                GameState.countBoss -= 1;
                //Destroy(gameObject);
                BulletPatternController.Instance.StopBossPattern();
                gameObject.SetActive(false);
            }
        }
    }

    void UpdateHP()
    {
        // cap nhat mau
        indexHealth = GetHealthPercent();
        if (hpBarProcess != null)
            hpBarProcess.Value = indexHealth;
    }

    private float GetHealthPercent()
    {
        if (maxHeath <= 0)
            return 0f;

        return Mathf.Clamp01(heath / (float)maxHeath) * 100f;
    }

    private void ApplyDamage(int damage)
    {
        if (damage <= 0)
        {
            UpdateHP();
            return;
        }

        heath = Mathf.Max(0, heath - damage);
        UpdateHP();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollis)
        {
            if (other.CompareTag(ListTag.TAG_DANTA))
            {
                ExplosionControl explosion = GameManager.Instance.explosionE01Pool.New();
                explosion.SetInfo(other.transform.position);

                if (def == MyPlaneController.instance.att)
                {
                    ApplyDamage(30);
                }
                else if (def > MyPlaneController.instance.att)
                {
                    ApplyDamage(20);
                }
                else if (def < MyPlaneController.instance.att)
                {
                    ApplyDamage(MyPlaneController.instance.att - def);
                }
                other.gameObject.SetActive(false);
            }
            if (other.CompareTag(ListTag.TAG_DANTA2))
            {
                ExplosionControl explosion = GameManager.Instance.explosionE01Pool.New();
                explosion.SetInfo(other.transform.position);

                ApplyDamage((int)(GameState.dmgSupport / 4));
                other.gameObject.SetActive(false);
            }
            if (other.CompareTag(ListTag.TAG_DAN5))
            {
                ExplosionControl explosion = GameManager.Instance.explosionFix2Pool.New();
                explosion.SetInfo(transform.position);
                ApplyDamage(GameState.dmgRocket);
                other.gameObject.SetActive(false);
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(ListTag.TAG_DAN3))
        {
            if (!onStayDmg)
            {
                StartCoroutine(StayDamage(100));
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag(ListTag.TAG_BULLET9))
        {
            if (!onStayDmg)
            {
                if (def == MyPlaneController.instance.att)
                {
                    ApplyDamage(100);
                }
                else if (def > MyPlaneController.instance.att)
                {
                    ApplyDamage(70);
                }
                else if (def < MyPlaneController.instance.att)
                {
                    ApplyDamage((MyPlaneController.instance.att - def) * 2);
                }
                StartCoroutine(StayDamage(0));
                onStayDmg = true;
            }
        }
        if (other.gameObject.CompareTag("danta26"))
        {
            if (!onStayDmg)
            {
                ApplyDamage(30);
                StartCoroutine(StayDamage(0));
                onStayDmg = true;
            }
        }
    }
    IEnumerator StayDamage(int dmge)
    {
        ExplosionControl explosion = GameManager.Instance.explosionE01Pool.New();
        explosion.SetInfo(transform.position);
        ApplyDamage(dmge);
        yield return waitStayDmg;
        onStayDmg = false;
    }

}
