using UnityEngine;
using TMPro;
using System.Collections;

public class UITextControl : MonoBehaviour
{
    public static UITextControl instance;
    public static UITextControl Instance
    {
        get { return instance; }
    }
    public TMP_Text scoreTxt;
    public TMP_Text hitTxt;

    public TMP_Text rocketTxt;
    public TMP_Text supportTxt;
    public TMP_Text shieldTxt;

    public GameObject popupEnoughItem;

    public TMP_Text levelTxt;
    private Vector3 rewardScale = new Vector3(1.6f, 1.6f, 1.6f);
    private int counter1 = 0;
    private int counter2 = 0;
    private WaitForSeconds waitRestoreText;
    void Awake()
    {
        instance = this;
        waitRestoreText = new WaitForSeconds(0.1f);
    }

    private void OnEnable()
    {
        GameState.BoostCountChanged += OnBoostCountChanged;
        RefreshBoostTexts();
    }

    private void OnDisable()
    {
        GameState.BoostCountChanged -= OnBoostCountChanged;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    void Start()
    {
        int level = GameState.currentLevel;
        GameState.lastCoin.Value = GameState.GetGold();
        if (levelTxt != null)
            levelTxt.text = level.ToString();
        // thong so diem
        if (scoreTxt != null)
        {
            scoreTxt.text = "000 000 000";
            //goldTxt.text = GameState.lastCoin.Value.ToString();
            if (hitTxt != null)
                hitTxt.text = GameState.hit.ToString();
            //  cap nhat so luong cua may nut an
            RefreshBoostTexts();
        }
    }

    private void OnBoostCountChanged(int rocketCount, int supportCount, int shieldCount)
    {
        SetBoostTexts(rocketCount, supportCount, shieldCount);
    }

    public void RefreshBoostTexts()
    {
        SetBoostTexts(GameState.totalRocketClick.Value, GameState.totalSupportClick.Value, GameState.totalShieldClick.Value);
    }

    private void SetBoostTexts(int rocketCount, int supportCount, int shieldCount)
    {
        if (rocketTxt != null)
            rocketTxt.text = rocketCount.ToString();
        if (supportTxt != null)
            supportTxt.text = supportCount.ToString();
        if (shieldTxt != null)
            shieldTxt.text = shieldCount.ToString();
    }

    public void CheckHitReward()
    {
        if (GameState.hit == 10 && counter1 == 0)
        {
            GameState.AddScore(25);
            counter1++;

            SetScoreHitScale(rewardScale, rewardScale);
            StartCoroutine(RestoreText());
        }
        if (GameState.hit == 20 && counter2 == 0)
        {
            GameState.AddScore(50);


            counter2++;

            SetScoreHitScale(rewardScale, rewardScale);
            StartCoroutine(RestoreText());
        }
        if (GameState.hit == 30 && counter2 == 0)
        {
            GameState.AddScore(50);


            counter2++;

            SetScoreHitScale(rewardScale, rewardScale);
            StartCoroutine(RestoreText());
        }
        if (GameState.hit == 40 && counter2 == 0)
        {
            GameState.AddScore(50);


            counter2++;

            SetScoreHitScale(rewardScale, rewardScale);
            StartCoroutine(RestoreText());
        }
        if (GameState.hit == 60 && counter2 == 0)
        {
            GameState.AddScore(50);

            counter2++;

            SetScoreHitScale(rewardScale, rewardScale);
            StartCoroutine(RestoreText());
        }
        if (GameState.hit > 60)
        {
            counter1 = 0;
            counter2 = 0;
            GameState.hit = 0;

            SetScoreHitScale(rewardScale, rewardScale);
            StartCoroutine(RestoreText());
        }
    }
    IEnumerator RestoreText()
    {
        yield return waitRestoreText;
        SetScoreHitScale(new Vector3(0.9f, 0.9f, 0.9f), new Vector3(1f, 1f, 1f));

    }

    private void SetScoreHitScale(Vector3 hitScale, Vector3 scoreScale)
    {
        if (hitTxt != null)
            hitTxt.transform.localScale = hitScale;
        if (scoreTxt != null)
            scoreTxt.transform.localScale = scoreScale;
    }

    public void ClickWin()
    {
        GameState.won = true;
    }

    public void ClickLose()
    {
        GameState.lose = true;
    }

    //public void UpdateGold()
    //{
    //    if (goldTxt != null)
    //        goldTxt.text = GameState.GetGold().ToString();
    //}
}
