using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UIUpgradeControl : MonoBehaviour
{
    public static UIUpgradeControl instance;

    public GameObject[] main;

    public Button upgradeGold;
    public Button upgradeCrystal;
    public Button returnToGeneralButton;
    public TMP_Text costGold;
    public TMP_Text costCrystal;
    public TMP_Text level;
    public TMP_Text txtName;
    public TMP_Text txtLevel;
    public Button btnOKFull;
    public Button btnYes;
    public Button btnNo;
    public GameObject banner;
    public TMP_Text notEnoughMessage;
    public GameObject fullBanner;
    public int currentLevel = 1;

    public GameObject showCanvas;
    public GameObject planePreviewRoot;
    public GameObject upgradeExplosion;
    public GameObject textUpgrade;
    public GameObject bgSpeedAtt;
    public GameObject groupWaitUpgrade;
    public GameObject panelSkipTime;
    public TMP_Text txtSpeedAtt;
    public TMP_Text txtCrystalSkip;
    public IndexPlane generalDialogIndex;
    public float upgradeVfxDuration = 0.8f;
    public float clickCooldownDuration = 0.2f;
    public float notEnoughPopupDuration = 0.8f;

    private int goldUpgrade;
    private int crystalUpgrade;
    bool isClickUpgrade;
    private WaitForSeconds waitUpgrade045;
    private WaitForSeconds waitClick;
    private WaitForSeconds waitHideNotEnoughPopup;

    private Coroutine hideNotEnoughPopupCoroutine;
    private Coroutine hideUpgradeVfxCoroutine;
    void Awake()
    {
        instance = this;
        waitUpgrade045 = new WaitForSeconds(0.45f);
        waitClick = new WaitForSeconds(clickCooldownDuration);
        waitHideNotEnoughPopup = new WaitForSeconds(notEnoughPopupDuration);
    }
    void Start()
    {
        SetActiveIfPresent(banner, false);
        AddClick(upgradeGold, UpgradeClickGold);
        AddClick(upgradeCrystal, UpgradeClickCrystal);
        AddClick(returnToGeneralButton, ReturnToGeneralDialog);
        AddClick(btnOKFull, OkClick);
    }

    void OnDestroy()
    {
        RemoveClick(upgradeGold, UpgradeClickGold);
        RemoveClick(upgradeCrystal, UpgradeClickCrystal);
        RemoveClick(returnToGeneralButton, ReturnToGeneralDialog);
        RemoveClick(btnOKFull, OkClick);
    }

    void OnEnable()
    {
        isClickUpgrade = false;
        UseUnscaledAnimatorTime();
        SetActiveIfPresent(textUpgrade, true);

        RefreshPlaneUpgradeView();
    }

    private void RefreshPlaneUpgradeView()
    {
        int maxLevel = GetMaxUpgradeLevel();
        if (maxLevel <= 0)
            return;

        if (GameState.GetPlaneUpgrading() != 0)
        {
            currentLevel = Mathf.Clamp(GameState.GetLevelPlane(0) + 1, 1, maxLevel);
            SetActiveIfPresent(groupWaitUpgrade, true);
        }
        else
        {
            currentLevel = Mathf.Clamp(GameState.GetLevelPlane(0), 1, maxLevel);
            SetActiveIfPresent(groupWaitUpgrade, false);
        }

        SetText(txtLevel, "Level " + currentLevel.ToString());
        SetText(txtName, GameConfigService.Instance.GetPlayerPlane().namePlane);
        RefreshUpgradeTitle(maxLevel);
        CheckCostUpgrade();
        ShowPlanePreview();

        if (IndexPlaneUpdate.instance != null)
            IndexPlaneUpdate.instance.SetInfoIndex();
        RefreshGeneralDialog();
    }

    void OnDisable()
    {
        if (hideUpgradeVfxCoroutine != null)
        {
            StopCoroutine(hideUpgradeVfxCoroutine);
            hideUpgradeVfxCoroutine = null;
        }

        if (hideNotEnoughPopupCoroutine != null)
        {
            StopCoroutine(hideNotEnoughPopupCoroutine);
            hideNotEnoughPopupCoroutine = null;
        }

        SetActiveIfPresent(upgradeExplosion, false);
        SetActiveIfPresent(textUpgrade, false);
        SetActiveIfPresent(fullBanner, false);
        SetActiveIfPresent(banner, false);
        SetDefaultIndex();
    }

    public void SetAnimationMain()
    {
        RefreshUpgradeTitle(GetMaxUpgradeLevel());

        if (main != null && main.Length > 0)
            SetActiveIfPresent(main[0], true);
    }

    public IEnumerator ChangeUgrade()
    {
        PlayUpgradeVfx();

        if (GameState.GetPlaneUpgrading() != 0)
        {
            if (currentLevel == GameState.GetLevelPlane(0))
            {
                currentLevel++;
            }
            GameState.SetLevelPlane(GameState.GetPlaneUpgrading() - 1, currentLevel);
        }
        else
        {
            GameState.SetLevelPlane(0, currentLevel);
        }
        RefreshPlaneUpgradeView();
        yield return waitUpgrade045;

    }

    public void SetDefaultIndex()
    {
        if (main == null)
            return;

        for (int i = 0; i < main.Length; i++)
        {
            SetActiveIfPresent(main[i], false);
        }
    }

    void UpdateCost(PlayerPlaneConfigSO plane)
    {
        if (plane == null || plane.upgrade == null || currentLevel < 0 || currentLevel >= plane.upgrade.Count)
            return;

        goldUpgrade = plane.upgrade[currentLevel].gold;
        crystalUpgrade = plane.upgrade[currentLevel].crystal;

        if (plane.upgrade[currentLevel].speed != 0)
        {
            SetActiveIfPresent(bgSpeedAtt, true);
            SetText(txtSpeedAtt, "+" + plane.upgrade[currentLevel].speed.ToString());
        }
        else
            SetActiveIfPresent(bgSpeedAtt, false);

    }

    void UpgradeClickGold()
    {
        if (TryHandleTutorialUpgrade())
            return;

        if (!CanStartUpgradeClick())
            return;

        if (currentLevel >= GetMaxUpgradeLevel())
        {
            ShowFullLevelBanner();
            return;
        }

        int currentGold = GameState.GetGold();
        if (currentGold < goldUpgrade)
        {
            ShowNotEnoughPopup("NOT ENOUGH COIN");
            return;
        }

        GameState.SetGold(currentGold - goldUpgrade);
        GameState.lastCoin.Value = GameState.GetGold();
        CompleteUpgradeClick();
    }

    IEnumerator WaitClick()
    {
        yield return waitClick;
        isClickUpgrade = false;
    }

    private void BeginClickCooldown()
    {
        isClickUpgrade = true;
        StartCoroutine(WaitClick());
    }

    void UpgradeClickCrystal()
    {
        if (!CanStartUpgradeClick())
            return;

        if (currentLevel >= GetMaxUpgradeLevel())
        {
            ShowFullLevelBanner();
            return;
        }

        int currentCrystal = GameState.GetCrystal();
        if (currentCrystal < crystalUpgrade)
        {
            ShowNotEnoughPopup("NOT ENOUGH DIAMOND");
            return;
        }

        GameState.SetCrystal(currentCrystal - crystalUpgrade);
        GameState.lastCrystal.Value = GameState.GetCrystal();
        CompleteUpgradeClick();
    }
    public void OkClick()
    {
        SetActiveIfPresent(fullBanner, false);
    }

    private void ReturnToGeneralDialog()
    {
        gameObject.SetActive(false);
        SetActiveIfPresent(showCanvas, true);
    }

    void CheckCostUpgrade()
    {
        if (currentLevel < GetMaxUpgradeLevel())
        {
            UpdateCost(GameConfigService.Instance.GetPlayerPlane());

            SetText(costGold, goldUpgrade.ToString());
            SetText(costCrystal, crystalUpgrade.ToString());
        }
        else
        {
            SetText(costGold, "LIMITED");
            SetText(costCrystal, "LIMITED");
            SetActiveIfPresent(bgSpeedAtt, false);
        }
    }

    private int GetMaxUpgradeLevel()
    {
        return GameConfigService.Instance.GetPlayerPlaneUpgradeCount();
    }

    private void RefreshGeneralDialog()
    {
        if (generalDialogIndex != null)
        {
            generalDialogIndex.SetInfoIndex();
            return;
        }

        if (IndexPlane.instance != null)
            IndexPlane.instance.SetInfoIndex();
    }

    private void ShowPlanePreview()
    {
        SetActiveIfPresent(planePreviewRoot, true);
        PlayerPlaneSpriteApplicator.ApplyToMenuPreviews(GameConfigService.Instance.GetPlayerPlane());

        if (main != null && main.Length > 0)
            SetActiveIfPresent(main[0], true);
    }

    private void UseUnscaledAnimatorTime()
    {
        SetAnimatorUpdateMode(planePreviewRoot);

        if (main != null)
        {
            for (int i = 0; i < main.Length; i++)
                SetAnimatorUpdateMode(main[i]);
        }

        if (IndexPlaneUpdate.instance != null)
            SetAnimatorUpdateMode(IndexPlaneUpdate.instance.gameObject);
    }

    private static void SetAnimatorUpdateMode(GameObject target)
    {
        if (target == null)
            return;

        Animator animator = target.GetComponent<Animator>();
        if (animator != null)
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void PlayUpgradeVfx()
    {
        if (upgradeExplosion == null)
            return;

        if (hideUpgradeVfxCoroutine != null)
            StopCoroutine(hideUpgradeVfxCoroutine);

        upgradeExplosion.SetActive(false);
        upgradeExplosion.SetActive(true);
        hideUpgradeVfxCoroutine = StartCoroutine(HideUpgradeVfxAfterDelay());
    }

    private IEnumerator HideUpgradeVfxAfterDelay()
    {
        yield return new WaitForSeconds(upgradeVfxDuration);
        SetActiveIfPresent(upgradeExplosion, false);
        hideUpgradeVfxCoroutine = null;
    }

    private void RefreshUpgradeTitle(int maxLevel)
    {
        if (currentLevel < maxLevel)
            SetText(level, "UPGRADE LEVEL " + (currentLevel + 1).ToString() + "?");
        else
            SetText(level, "MAXIMUM LEVEL " + maxLevel.ToString());
    }

    public void ClickSkipUpgrade()
    {
        int index = GameState.GetLevelPlane(0) < 8 ? GameState.GetLevelPlane(0) : 7;

        SetText(txtCrystalSkip, GameplayTimers.COST_CRYSTAL_SKIP[index].ToString());
        SetActiveIfPresent(panelSkipTime, true);
    }

    public void ClickCrystalSkip()
    {
        int index = GameState.GetLevelPlane(0) < 8 ? GameState.GetLevelPlane(0) : 7;

        int currentCrystal = GameState.GetCrystal();
        if (currentCrystal >= GameplayTimers.COST_CRYSTAL_SKIP[index])
        {
            GameState.SetCrystal(currentCrystal - GameplayTimers.COST_CRYSTAL_SKIP[index]);
            GameState.lastCrystal.Value = GameState.GetCrystal();
            if (MenuEventListener.instance != null)
                MenuEventListener.instance.UpdateText();
            UpgradeSuccessful();
        }
    }

    public void UpgradeSuccessful()
    {
        SetActiveIfPresent(panelSkipTime, false);
        StartCoroutine(ChangeUgrade());
        GameplayTimers.timeCountUpgrade = 0;
        GameState.SetPlaneUpgrading(0);
        SetActiveIfPresent(groupWaitUpgrade, false);
    }

    public void ClickCloseSkip()
    {
        SetActiveIfPresent(panelSkipTime, false);
    }

    private bool IsWaitingForUpgrade()
    {
        return groupWaitUpgrade != null && groupWaitUpgrade.activeInHierarchy;
    }

    private bool TryHandleTutorialUpgrade()
    {
        if (TutorialController.instance == null || !TutorialController.instance.isTutorialLevel4)
            return false;

        currentLevel++;
        CheckCostUpgrade();
        StartCoroutine(ChangeUgrade());
        TutorialController.instance.CheckTutorialLevel4();
        return true;
    }

    private bool CanStartUpgradeClick()
    {
        if (isClickUpgrade)
            return false;

        if (!IsWaitingForUpgrade())
            return true;

        GameNotice.Show("Please wait while spaceship upgrade");
        return false;
    }

    private void CompleteUpgradeClick()
    {
        if (MenuEventListener.instance != null)
            MenuEventListener.instance.UpdateText();

        currentLevel++;
        StartCoroutine(ChangeUgrade());
        BeginClickCooldown();
    }

    private void ShowFullLevelBanner()
    {
        SetActiveIfPresent(fullBanner, true);
        BeginClickCooldown();
    }

    private void ShowNotEnoughPopup(string message)
    {
        SetText(notEnoughMessage, message);
        SetActiveIfPresent(btnYes != null ? btnYes.gameObject : null, false);
        SetActiveIfPresent(btnNo != null ? btnNo.gameObject : null, false);
        SetActiveIfPresent(banner, true);

        if (hideNotEnoughPopupCoroutine != null)
            StopCoroutine(hideNotEnoughPopupCoroutine);
        hideNotEnoughPopupCoroutine = StartCoroutine(HideNotEnoughPopupAfterDelay());
    }

    private IEnumerator HideNotEnoughPopupAfterDelay()
    {
        yield return waitHideNotEnoughPopup;
        SetActiveIfPresent(banner, false);
        hideNotEnoughPopupCoroutine = null;
    }

    private static void AddClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.AddListener(action);
    }

    private static void RemoveClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.RemoveListener(action);
    }

    private static void SetActiveIfPresent(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }

    private static void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value;
    }
}
