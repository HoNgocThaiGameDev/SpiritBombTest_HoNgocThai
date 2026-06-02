using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuEventListener : MonoBehaviour
{
    public static MenuEventListener instance;

    public Button fight;
    public Button back;
    public Button btnSetting;
    public Button upgrade;
    public Button itemShop;
    public Button btnInfo;
    public Button yesQuit;
    public Button noQuit;
    public Button[] extraUpgradePlaneButtons;
    public Button[] extraUpgradeBoostButtons;

    public TMP_Text txtCoin;
    public TMP_Text txtCrystal;
    public TMP_Text txtEnergy;
    public TMP_Text txtCounter;
    public GameObject showCanvas;
    public GameObject itemCanvas;
    public GameObject LastCanvas;
    public GameObject LastUpgradeCanvas;
    public GameObject canvasInfo;
    public GameObject panelInfo;
    public GameObject panelSetting;
    public GameObject quitPanel;

    private bool isBackClick;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        GameState.isGamePaused = false;

        AddClick(fight, FightClick);
        AddClick(back, BackClick);
        AddClick(btnSetting, ShowSetting);
        AddClick(btnInfo, ShowInfo);
        AddClick(yesQuit, YesClick);
        AddClick(noQuit, NoClick);
        AddClick(upgrade, UpgradeClick);
        AddClick(itemShop, ItemClick);
        AddClick(extraUpgradePlaneButtons, UpgradeClick);
        AddClick(extraUpgradeBoostButtons, ShowInfo);
        BindUpgradePlaneButtons();

        UpdateText();
        StartCoroutine(WaitClickBack());
    }

    private void OnDestroy()
    {
        RemoveClick(fight, FightClick);
        RemoveClick(back, BackClick);
        RemoveClick(btnSetting, ShowSetting);
        RemoveClick(btnInfo, ShowInfo);
        RemoveClick(yesQuit, YesClick);
        RemoveClick(noQuit, NoClick);
        RemoveClick(upgrade, UpgradeClick);
        RemoveClick(itemShop, ItemClick);
        RemoveClick(extraUpgradePlaneButtons, UpgradeClick);
        RemoveClick(extraUpgradeBoostButtons, ShowInfo);
    }

    private System.Collections.IEnumerator WaitClickBack()
    {
        yield return new WaitForSeconds(1);
        isBackClick = true;
    }

    private static void AddClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
        {
            button.onClick.RemoveListener(action);
            button.onClick.AddListener(action);
        }
    }

    private static void AddClick(Button[] buttons, UnityEngine.Events.UnityAction action)
    {
        if (buttons == null)
            return;

        for (int i = 0; i < buttons.Length; i++)
            AddClick(buttons[i], action);
    }

    private static void RemoveClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.RemoveListener(action);
    }

    private static void RemoveClick(Button[] buttons, UnityEngine.Events.UnityAction action)
    {
        if (buttons == null)
            return;

        for (int i = 0; i < buttons.Length; i++)
            RemoveClick(buttons[i], action);
    }

    private static void SetActiveIfPresent(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }

    private void BindUpgradePlaneButtons()
    {
        if (LastCanvas == null)
            return;

        Button[] buttons = LastCanvas.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            if (button == null)
                continue;

            Transform parent = button.transform.parent;
            bool isUpgradePlaneButton = button.gameObject.name == "ButtonUpgrade"
                || (parent != null && parent.gameObject.name == "ButtonUpgrade");

            if (isUpgradePlaneButton)
                AddClick(button, UpgradeClick);
        }
    }

    private void FightClick()
    {
        SceneFlowService.LoadSelectLevel();
    }

    private void ShowSetting()
    {
        SetActiveIfPresent(panelSetting, true);
    }

    private void ShowInfo()
    {
        SetActiveIfPresent(panelInfo, true);
        SetActiveIfPresent(canvasInfo, true);
    }

    private void UpgradeClick()
    {
        OpenPlaneUpgrade();
    }

    public void ItemClick()
    {
        SetActiveIfPresent(showCanvas, false);
        SetActiveIfPresent(itemCanvas, true);
    }

    public void CheckActive()
    {
        UpdateText();
    }

    public void CheckItemContent()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        int gold = GameState.GetGold();
        int crystal = GameState.GetCrystal();
        GameState.lastCoin.Value = gold;
        GameState.lastCrystal.Value = crystal;

        if (txtCoin != null)
            txtCoin.text = gold.ToString();
        if (txtCrystal != null)
            txtCrystal.text = crystal.ToString();
        if (txtEnergy != null)
            txtEnergy.text = GameState.GetEnergy().ToString();
        if (txtCounter != null && GameState.GetEnergy() >= 100)
            txtCounter.gameObject.SetActive(false);
    }

    public void UpdateTxtCounter(int time)
    {
        if (txtCounter == null)
            return;

        if (GameState.energyToPlay.Value < 100)
        {
            int minute = time / 60;
            int second = time - minute * 60;
            txtCounter.gameObject.SetActive(true);
            txtCounter.text = string.Format("{0:00}:{1:00}", minute, second);
        }
        else
        {
            txtCounter.gameObject.SetActive(false);
        }
    }

    public void YesClick()
    {
        Application.Quit();
    }

    public void NoClick()
    {
        SetActiveIfPresent(quitPanel, false);
    }

    private void BackClick()
    {
        if (!isBackClick)
            return;

        if (LastUpgradeCanvas != null && LastUpgradeCanvas.activeInHierarchy)
        {
            ClosePlaneUpgrade();
            return;
        }

        if (panelSetting != null && panelSetting.activeInHierarchy)
            panelSetting.SetActive(false);
        else if (panelInfo != null && panelInfo.activeInHierarchy)
            panelInfo.SetActive(false);
        else
            SetActiveIfPresent(quitPanel, true);
    }

    private void OpenPlaneUpgrade()
    {
        SetActiveIfPresent(LastCanvas, false);
        SetActiveIfPresent(LastUpgradeCanvas, true);
    }

    private void ClosePlaneUpgrade()
    {
        SetActiveIfPresent(LastUpgradeCanvas, false);
        SetActiveIfPresent(LastCanvas, true);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if (audioSource != null)
            audioSource.Play();
        BackClick();
    }
}
