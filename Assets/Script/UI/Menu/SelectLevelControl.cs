using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectLevelControl : MonoBehaviour
{
    private const int MaxPlayableLevel = 3;

    public static SelectLevelControl instance;

    public Button back;
    public Transform contentPos;
    public TMP_Text textCoin;
    public TMP_Text textCrystal;
    public TMP_Text textEnergy;
    public TMP_Text textCounter;
    public GameObject panelLevelSelected;
    public Button tapHide;
    public GameObject scrollView;
    public GameObject panelSetting;
    public TMP_Text txtStarCollected;
    public Transform[] levelPos;
    public GameObject bossFinal;

    private ScrollRect scrScrollView;
    private bool isClickBack;
    private bool isLoadingGameplay;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        GameState.isGamePaused = false;

        audioSource = GetComponent<AudioSource>();
        if (scrollView != null)
            scrScrollView = scrollView.GetComponent<ScrollRect>();

        if (txtStarCollected != null)
            txtStarCollected.text = GameState.GetStar().ToString() + "/180";

        UpdateText();
        UpdateTextEnergy(0);

        if (GameState.GetLevelCompleted() >= 6 && GameState.GetLevelCompleted() <= 57 && GameState.GetLevelCompleted() < levelPos.Length)
            contentPos.position = new Vector3(contentPos.position.x, -levelPos[GameState.GetLevelCompleted()].position.y - 4, 0);
        else if (GameState.GetLevelCompleted() > 57)
            contentPos.position = new Vector3(contentPos.position.x, -54f, 0);

        RegisterListeners();

        StartCoroutine(WaitClickBack());
        ApplyVisibleLevelLabels();
        TrimUnavailableLevelButtons();
    }

    private void OnEnable()
    {
        ApplyVisibleLevelLabels();
    }

    private void OnDestroy()
    {
        UnregisterListeners();
    }

    private void ApplyVisibleLevelLabels()
    {
        if (levelPos == null)
            return;

        int visibleLevelCount = Mathf.Min(MaxPlayableLevel, levelPos.Length);
        for (int i = 0; i < visibleLevelCount; i++)
            SetLevelLabel(levelPos[i], i + 1);
    }

    private void SetLevelLabel(Transform levelTransform, int levelNumber)
    {
        if (levelTransform == null)
            return;

        TMP_Text[] labels = levelTransform.GetComponentsInChildren<TMP_Text>(true);
        if (labels == null || labels.Length == 0)
            return;

        string levelText = levelNumber.ToString();
        for (int i = 0; i < labels.Length; i++)
        {
            TMP_Text label = labels[i];
            if (label == null)
                continue;

            string currentText = string.IsNullOrEmpty(label.text) ? string.Empty : label.text.Trim();
            if (currentText.Equals("Tutorial", System.StringComparison.OrdinalIgnoreCase) ||
                currentText.Equals(levelText, System.StringComparison.OrdinalIgnoreCase) ||
                currentText.Equals("Level " + levelText, System.StringComparison.OrdinalIgnoreCase) ||
                currentText.Length <= 2)
            {
                label.text = levelText;
                return;
            }
        }
    }

    private void TrimUnavailableLevelButtons()
    {
        if (levelPos != null)
        {
            for (int i = MaxPlayableLevel; i < levelPos.Length; i++)
            {
                if (levelPos[i] != null)
                    Destroy(levelPos[i].gameObject);
            }
        }

        if (bossFinal != null)
            Destroy(bossFinal);
    }

    private IEnumerator WaitClickBack()
    {
        yield return new WaitForSeconds(1);
        isClickBack = true;
    }

    public void UpdateText()
    {
        if (textCoin != null)
            textCoin.text = GameState.GetGold().ToString();
        if (textCrystal != null)
            textCrystal.text = GameState.GetCrystal().ToString();
        if (textEnergy != null)
            textEnergy.text = GameState.GetEnergy().ToString();
        if (textCounter != null && GameState.GetEnergy() >= 100)
            textCounter.gameObject.SetActive(false);
    }

    public void UpdateTextEnergy(int time)
    {
        if (textCounter == null)
            return;

        if (GameState.energyToPlay.Value < 100)
        {
            int minute = time / 60;
            int second = time - minute * 60;
            textCounter.gameObject.SetActive(true);
            textCounter.text = string.Format("{0:00}:{1:00}", minute, second);
        }
        else
        {
            textCounter.gameObject.SetActive(false);
        }
    }

    public void LoadMenu()
    {
        if (!isClickBack)
            return;

        SceneFlowService.LoadMenu(false);
    }

    public void ClickLevel(int level)
    {
        if (isLoadingGameplay)
            return;

        if (level < 1 || level > MaxPlayableLevel)
        {
            Debug.LogWarning("[SelectLevelControl] Invalid level selected: " + level);
            return;
        }

        if (!GameState.LevelFull && level > GameState.GetLevelCompleted() + 1)
            return;

        GameState.currentLevel.Value = level;
        UseUnscaledAnimatorTime(panelLevelSelected);
        if (panelLevelSelected != null)
            panelLevelSelected.SetActive(true);
        if (scrScrollView != null)
            scrScrollView.vertical = false;
    }

    public void ClickFight()
    {
        if (isLoadingGameplay)
            return;

        int energyCost = GetSelectedLevelEnergyCost();
        if (!TrySpendEnergy(energyCost))
            return;

        isLoadingGameplay = true;
        GameState.levelLose = GameState.currentLevel;
        SceneFlowService.LoadGameplay(true);
    }

    private int GetSelectedLevelEnergyCost()
    {
        try
        {
            return GameConfigService.Instance.GetLevelEnergyCost(GameState.currentLevel.Value);
        }
        catch (System.Exception exception)
        {
            Debug.LogWarning("[SelectLevelControl] Could not read energy cost for level " + GameState.currentLevel.Value + ": " + exception.Message);
            return 0;
        }
    }

    private bool TrySpendEnergy(int energyCost)
    {
        if (energyCost <= 0)
            return true;

        int currentEnergy = GameState.GetEnergy();
        if (currentEnergy < energyCost)
            return false;

        GameState.energyToPlay.Value = currentEnergy - energyCost;
        GameState.SaveEnergy();
        UpdateText();
        UpdateTextEnergy(0);
        return true;
    }

    public void ClickSetting()
    {
        if (panelSetting != null)
            panelSetting.SetActive(true);
    }

    public void TapHide()
    {
        if (panelLevelSelected != null)
            panelLevelSelected.SetActive(false);
        if (scrScrollView != null)
            scrScrollView.vertical = true;
    }

    private void RegisterListeners()
    {
        AddClick(back, LoadMenu);
        AddClick(tapHide, TapHide);
    }

    private void UnregisterListeners()
    {
        RemoveClick(back, LoadMenu);
        RemoveClick(tapHide, TapHide);
    }

    private static void AddClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null)
            return;

        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    private static void RemoveClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.RemoveListener(action);
    }

    private static void UseUnscaledAnimatorTime(GameObject target)
    {
        if (target == null)
            return;

        Animator[] animators = target.GetComponentsInChildren<Animator>(true);
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i] != null)
                animators[i].updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) || !isClickBack)
            return;

        if (audioSource != null)
            audioSource.Play();

        bool levelSelectedOpen = panelLevelSelected != null && panelLevelSelected.activeInHierarchy;
        bool settingsOpen = panelSetting != null && panelSetting.activeInHierarchy;

        if (!levelSelectedOpen && !settingsOpen)
        {
            LoadMenu();
            return;
        }

        if (levelSelectedOpen)
            TapHide();
        else if (settingsOpen)
            panelSetting.SetActive(false);
    }
}
