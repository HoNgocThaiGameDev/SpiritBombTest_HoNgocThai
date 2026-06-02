using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinControl : MonoBehaviour
{
    public static WinControl instance;
    public TMP_Text txtGold;
    public TMP_Text txtCrystal;
    public TMP_Text txtKill;
    public TMP_Text txtScore;
    public TMP_Text txtBestScore;
    public Button btnRetry;
    public Button btnNextLevel;
    public Button btnHome;
    public GameObject Add2Crystal;
    public Animator animResult;

    void OnEnable()
    {
        instance = this;
        AddListener(btnRetry, ClickRetry);
        AddListener(btnHome, ClickHome);
        AddListener(btnNextLevel, ClickNextLevel);
    }
    void Start()
    {
        GamePlayEventListener.instance.TopCanvas.SetActive(false);

        if (Add2Crystal != null)
            Add2Crystal.SetActive(false);
        MyPlaneController.instance.GetComponent<CircleCollider2D>().enabled = false;


        if (GamePlayEventListener.instance.countSaveMeRevive == 0)
        {
            if (GameState.won)
            {
                GameState.countLose = 0;

                GameState.countWin++;
                if (GameState.countWin >= 4)
                {
                    GameState.countWin = 0;
                }
            }
        }

        BulletPatternController.Instance.StopAllPatterns();

        if (GameState.won)
        {
            CameraControl.instance.missionComplete.SetActive(false);
            GameState.SaveHighScore();
            GameState.numberLoseLevel = 0;
        }

        if (GameState.lose)
        {
            CameraControl.instance.missionLose.SetActive(false);
            GameState.loseCount++;
            GameState.numberLoseLevel++;
        }
        RefreshResultTexts();

        GameState.SaveCoin();
        GameState.SaveCrystal();
        GameState.SaveEnergy();
        GameState.SaveItem();
    }

    void OnDisable()
    {
        RemoveListener(btnRetry, ClickRetry);
        RemoveListener(btnHome, ClickHome);
        RemoveListener(btnNextLevel, ClickNextLevel);
        if (instance == this)
            instance = null;
    }

    private static void AddListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null)
            return;

        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    private static void RemoveListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.RemoveListener(action);
    }

    private static float GetKillPercentage()
    {
        int totalEnemy = GameState.currentTotalEnemy.Value;
        if (totalEnemy <= 0)
            return 0f;

        return Mathf.Clamp(100f * GameState.totalHit.Value / totalEnemy, 0f, 100f);
    }

    void ClickRetry()
    {
        EnableResultAnimation();
        SceneFlowService.RetryGameplay();
    }
    void ClickNextLevel()
    {
        EnableResultAnimation();
        SceneFlowService.LoadSelectLevel();
        GameState.loseCount = 0;
        GameState.won = false;
        GameState.lose = false;
        if (SoundController.instance != null)
            SoundController.instance.PlaySound(8);
    }
    void ClickHome()
    {
        GamePlayEventListener.instance.RightCanvas.SetActive(false);
        EnableResultAnimation();
        SceneFlowService.LoadMenu(false);
        GameState.won = false;
        GameState.lose = false;
        if (SoundController.instance != null)
            SoundController.instance.PlaySound(8);
    }

    private void RefreshResultTexts()
    {
        SetText(txtGold, GameState.currentCoin.Value.ToString());
        SetText(txtCrystal, GameState.currentCrystal.ToString());
        SetText(txtKill, string.Format("{0:0.0}%", GetKillPercentage()));
        SetText(txtScore, GameState.lastscore.Value.ToString());
        SetText(txtBestScore, GameState.GetTotalHighScore().ToString());
    }

    private void EnableResultAnimation()
    {
        if (animResult != null)
            animResult.enabled = true;
    }

    private static void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value;
    }
}
