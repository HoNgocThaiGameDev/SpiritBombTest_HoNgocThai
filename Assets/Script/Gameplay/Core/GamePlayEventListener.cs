using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePlayEventListener : MonoBehaviour
{
    public static GamePlayEventListener instance;
    public GameObject bgLevel;
    public Button support;
    public Button rocket;
    public Button shield;
    public GameObject[] laserSupport6;
    public GameObject[] supportPlane;
    Vector2 hotong1; // may bay ho tong ben phai 
    Vector2 hotong2; // may bay ho tong ben trai
    public GameObject pausePanel;
    public GameObject winPopup;
    public GameObject losePopup;
    bool fireRocket;

    public Image mask1; // support
    public Image mask2; // shield
    public Image mask3;  // rocket

    float rocketTime;   //25
    float shieldTime;   //15
    float supportTime;  //15
    float timeR;
    float timeS;
    float timeSup;
    public GameObject TopCanvas;
    public GameObject RightCanvas;
    public Button okPopupNotEnough;
    public GameObject popupNotEnough;
    public GameObject saveMePanel;

    private WaitForSeconds waitDisableSupport;
    private WaitForSeconds waitRestoreBullet;
    public bool isSaveMeUsed;
    public GameObject saveMeDone;
    private GameObject activeSupportPlane;

    public int countSaveMeRevive = 0;
    public float indexHard = 0;

    void Awake()
    {
        instance = this;
        isSaveMeUsed = false;
        int levelSupport = GameState.levelSupport <= 5 ? GameState.levelSupport : 5;

        SupportSOData supportUpgrade = GameConfigService.Instance.GetSupportUpgrade(levelSupport);
        waitDisableSupport = new WaitForSeconds(supportUpgrade.duration);
        waitRestoreBullet = new WaitForSeconds(10);
        GameState.dmgSupport.Value = supportUpgrade.attack;
        if (TopCanvas != null)
            TopCanvas.SetActive(true);
        GameState.ResetAll();
        SetValueMissile();
        //SetValueShield();
        SetValueSupport();
        if (GameState.currentLevel <= 20)
        {
            indexHard = 1;
        }
        else if (GameState.currentLevel > 20 && GameState.currentLevel < 30)
        {
            indexHard = 1.2f;
        }
        else if (GameState.currentLevel >= 30 && GameState.currentLevel < 40)
        {
            indexHard = 1.5f;
        }
        else if (GameState.currentLevel >= 40 && GameState.currentLevel < 50)
        {
            indexHard = 1.8f;
        }
        else if (GameState.currentLevel >= 50)
        {
            indexHard = 2f;
        }
    }

    private void OnEnable()
    {
        RegisterListeners();
    }

    private void OnDisable()
    {
        UnregisterListeners();
        if (instance == this)
            instance = null;
    }

    private void RegisterListeners()
    {
        AddListener(okPopupNotEnough, OkPopupNotEnough);
        AddListener(support, SupportClick);
        AddListener(rocket, RocketClick);
        AddListener(shield, ShieldClick);
    }

    private void UnregisterListeners()
    {
        RemoveListener(okPopupNotEnough, OkPopupNotEnough);
        RemoveListener(support, SupportClick);
        RemoveListener(rocket, RocketClick);
        RemoveListener(shield, ShieldClick);
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

    void OkPopupNotEnough()
    {
        popupNotEnough.SetActive(false);
    }
    IEnumerator Start()
    {
        if (SoundController.instance != null)
            SoundController.instance.StopSound(8);
        fireRocket = MyPlaneController.instance.fireRocket;
        if (SoundController.instance != null)
        {
            SoundController.instance.StopAllSound();
        }
        yield return new WaitForSeconds(1);
        if (SoundController.instance != null)
        {
            if (GameState.currentLevel <= 8)
            {
                SoundController.instance.PlaySound(Random.Range(0, 2));
            }
            else if (GameState.currentLevel >= 9 && GameState.currentLevel <= 23)
            {
                SoundController.instance.PlaySound(2);
            }
            else if (GameState.currentLevel >= 24 && GameState.currentLevel <= 38)
            {
                SoundController.instance.PlaySound(Random.Range(3, 5));
            }
            else if (GameState.currentLevel >= 39 && GameState.currentLevel <= 52)
            {
                SoundController.instance.PlaySound(Random.Range(5, 7));
            }
            else if (GameState.currentLevel >= 53 && GameState.currentLevel <= 60)
            {
                SoundController.instance.PlaySound(7);
            }
        }
    }

    void PlayClick()
    {
        SceneFlowService.RetryGameplay();
    }

    void ShieldClick()
    {
        // shield control

        var shieldObj = MyPlaneController.instance.shield;
        if (GameState.totalShieldClick <= 0)
        {
            GameState.buyMoreItem = 3;
            ShowNotEnoughItemPopup();
        }
        if (shieldObj.activeInHierarchy == false && GameState.totalShieldClick > 0 && GameState.isGamePaused == false)
        {
            if (mask2.IsActive())
            {
                // none
            }
            else
            {
                shieldTime = GameConfigService.Instance.GetShieldUpgrade(GameState.levelShield).duration;
                timeS = shieldTime;
                mask2.gameObject.SetActive(true);
                GameState.SetShield(GameState.totalShieldClick.Value - 1);
                SetValueShield();
                shieldObj.SetActive(true);
                GameState.useShield.Value += 1;
            }
        }
    }

    void RocketClick()
    {
        if (GameState.totalRocketClick <= 0)
        {
            GameState.buyMoreItem = 1;
            ShowNotEnoughItemPopup();
        }
        if (MyPlaneController.instance.fireRocket == false && GameState.isGamePaused == false && GameState.totalRocketClick > 0)
        {
            // kiem tra xem co dang countdown hay khong
            if (mask3.IsActive())
            {
                // khong lam gi
            }
            else
            {
                mask3.gameObject.SetActive(true);
                rocketTime = 10;
                timeR = rocketTime;
                GameState.SetRocket(GameState.totalRocketClick.Value - 1);
                MyPlaneController.instance.fireRocket = true;
                StartCoroutine(RestoreBullet());
                GameState.useRocket.Value += 1;
            }
        }
    }

    public void ClickPause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            GameState.isGamePaused = true;
            pausePanel.SetActive(true);
        }
    }
    void SupportClick()
    {
        if (GameState.totalSupportClick <= 0)
        {
            GameState.buyMoreItem = 2;
            ShowNotEnoughItemPopup();
        }
        GameObject supportBoost = GetSupportBoostPlane();
        if (supportBoost == null)
        {
            Debug.LogWarning("[GamePlayEventListener] Missing support boost plane.");
            return;
        }

        if (!IsSupportBoostActive() && GameState.totalSupportClick > 0 && GameState.isGamePaused == false)
        {
            if (mask1.IsActive())
            {
                // none
            }
            else
            {
                mask1.gameObject.SetActive(true);
                supportTime = GameConfigService.Instance.GetSupportUpgrade(GameState.levelSupport).duration;
                timeSup = supportTime;
                GameState.SetSupport(GameState.totalSupportClick.Value - 1);
                SetSupportBoostActive(true);
                StartCoroutine(DisableSupport());
                GameState.useSupport.Value += 1;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0 && MyPlaneController.instance.energy <= 0)
            {
                GameState.SaveCoin();
                GameState.SaveCrystal();
                GameState.SaveItem();
                SceneFlowService.LoadMenu(false);
                SoundController.instance.StopAllSound();
                SoundController.instance.PlaySound(8);
            }
            else if (PopupManager.instance.popup.activeInHierarchy)
            {
                PopupManager.instance.popup.SetActive(false);
            }
            else if (GetNotEnoughItemPopup() != null && GetNotEnoughItemPopup().activeInHierarchy)
            {
                GetNotEnoughItemPopup().SetActive(false);
            }
            else if (popupNotEnough.activeInHierarchy)
            {
                popupNotEnough.SetActive(false);
            }
            else if (MyPlaneController.instance.energy > 0 && !GameState.lose && !GameState.won)
            {
                if (Time.timeScale == 1)
                {
                    Time.timeScale = 0;
                    AudioListener.pause = true;
                    GameState.isGamePaused = true;
                    pausePanel.SetActive(true);
                }
                else
                {
                    pausePanel.SetActive(false);
                    if (AudioListener.pause && PlayerSettingsService.IsSoundEnabled())
                    {
                        AudioListener.pause = false;
                    }
                    GameState.isGamePaused = false;
                    Time.timeScale = 1;
                }
            }
        }

        #region countdown item

        if (mask3.IsActive())
        {
            rocketTime -= Time.deltaTime;
            mask3.fillAmount = rocketTime / timeR;
            if (mask3.fillAmount < 0.02f)
            {
                mask3.gameObject.SetActive(false);
                MyPlaneController.instance.fireRocket = false;
            }
        }
        if (mask2.IsActive())
        {
            shieldTime -= Time.deltaTime;
            mask2.fillAmount = shieldTime / timeS;
            if (mask2.fillAmount < 0.02f)
            {
                mask2.gameObject.SetActive(false);
                MyPlaneController.instance.shield.SetActive(false);
                MyPlaneController.instance.SetShieldBarValue(0f);
            }
        }
        if (mask1.IsActive())
        {
            supportTime -= Time.deltaTime;
            mask1.fillAmount = supportTime / timeSup;
            if (mask1.fillAmount < 0.02f)
            {
                mask1.gameObject.SetActive(false);
                SetSupportBoostActive(false);
            }
        }

        #endregion
    }
    void OnApplicationPause(bool isPause)
    {
        if (!winPopup.activeInHierarchy && !losePopup.activeInHierarchy && !GameState.won
            && !GameState.lose && !GameState.isSaveMe)
        {
            if (isPause)
            {
                AudioListener.pause = true;
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    IEnumerator DisableSupport()
    {
        yield return waitDisableSupport;
        MyPlaneController.instance.isSupport6Att = false;
        SetSupportBoostActive(false);
    }

    public void SetSupportBoostActive(bool active)
    {
        GameObject supportBoost = GetSupportBoostPlane();
        if (supportBoost == null)
            return;

        SetSupportChildrenVisible(supportBoost.transform, active);
        supportBoost.SetActive(active);
    }

    private bool IsSupportBoostActive()
    {
        GameObject supportBoost = GetSupportBoostPlane();
        return supportBoost != null && supportBoost.activeInHierarchy;
    }

    private GameObject GetSupportBoostPlane()
    {
        if (activeSupportPlane != null)
            return activeSupportPlane;

        if (supportPlane != null)
        {
            for (int i = 0; i < supportPlane.Length; i++)
            {
                if (supportPlane[i] != null)
                {
                    activeSupportPlane = supportPlane[i];
                    return activeSupportPlane;
                }
            }
        }

        if (MyPlaneController.instance != null && MyPlaneController.instance.planeSkeleton != null)
        {
            for (int i = 0; i < MyPlaneController.instance.planeSkeleton.Length; i++)
            {
                GameObject plane = MyPlaneController.instance.planeSkeleton[i];
                if (plane == null)
                    continue;

                Transform supportTransform = plane.transform.Find("Support");
                if (supportTransform != null)
                {
                    activeSupportPlane = supportTransform.gameObject;
                    return activeSupportPlane;
                }
            }
        }

        return null;
    }

    private void SetSupportChildrenVisible(Transform root, bool visible)
    {
        if (root == null)
            return;

        for (int i = 0; i < root.childCount; i++)
        {
            Transform child = root.GetChild(i);
            child.gameObject.SetActive(visible);

            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.enabled = visible;
                Color color = renderer.color;
                color.a = 1f;
                renderer.color = color;
            }

            SetSupportChildrenVisible(child, visible);
        }
    }

    private void ShowNotEnoughItemPopup()
    {
        GameObject popup = GetNotEnoughItemPopup();
        if (popup != null)
            popup.SetActive(true);
    }

    private GameObject GetNotEnoughItemPopup()
    {
        UITextControl textControl = UITextControl.Instance != null
            ? UITextControl.Instance
            : FindObjectOfType<UITextControl>();
        return textControl != null ? textControl.popupEnoughItem : null;
    }

    IEnumerator RestoreBullet()
    {
        yield return waitRestoreBullet;
        MyPlaneController.instance.fireRocket = false;
    }
    //Missile
    public void SetValueMissile()
    {
        int lvMissile = GameState.levelMissile <= 5 ? GameState.levelMissile : 5;

        MissileSOData missileUpgrade = GameConfigService.Instance.GetMissileUpgrade(lvMissile);
        GameState.dmgRocket.Value = missileUpgrade.attack;
        GameState.speedRocket.Value = missileUpgrade.speed;
    }
    //shield
    public void SetValueShield()
    {
        GameState.shield.Value = GameConfigService.Instance.GetShieldUpgrade(GameState.levelShield).defend;
        MyPlaneController.instance.totalShield = GameState.shield;
        MyPlaneController.instance.SetShieldBarValue(1f);
        //MyPlaneController.instance.UpdateShield(-GameState.shield);
    }
    //support
    public void SetValueSupport()
    {
        int levelSupport = GameState.levelSupport <= 5 ? GameState.levelSupport : 5;

        SupportSOData supportUpgrade = GameConfigService.Instance.GetSupportUpgrade(levelSupport);
        GameState.dmgSupport.Value = supportUpgrade.attack;
        GameState.speedSupport.Value = supportUpgrade.speed;
    }

    public void ReviveFromSaveMe()
    {
        isSaveMeUsed = true;
        countSaveMeRevive++;
        MyPlaneController.instance.gameObject.SetActive(true);
        MyPlaneController.instance.CheckBullet();
        MyPlaneController.instance.onStayDmg = false;
        MyPlaneController.instance.ResetSpawnProtection();
        MyPlaneController.instance.energy = GameState.energyPlane;
        MyPlaneController.instance.UpdateEnergy(-MyPlaneController.instance.totalEnergy);
        saveMePanel.SetActive(false);
        GameState.isGamePaused = false;
        GameState.isMoveMap = true;
        Time.timeScale = 1;
        GameState.totalSaveMe.Value++;
        GameState.countSaveMe.Value++;
        SetValueShield();
        MyPlaneController.instance.totalShield = GameState.shield; //get totalShield
        MyPlaneController.instance.shield.SetActive(true);

    }
}
