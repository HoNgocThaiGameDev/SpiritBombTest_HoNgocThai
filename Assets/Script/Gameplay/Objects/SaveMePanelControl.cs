using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SaveMePanelControl : MonoBehaviour
{
    private const int StartCountdownValue = 5;
    private const int DefaultSaveCost = 1;

    public TMP_Text countdownText;
    public Button saveButton;
    public Button noButton;
    public GameObject popupEnough;
    public TMP_Text costText;

    private Coroutine countdownCoroutine;
    private AudioSource audioSource;
    private int countdownValue = StartCountdownValue;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AddListener(saveButton, ClickSave);
        AddListener(noButton, ClickNo);
        Time.timeScale = 0f;
        GameState.isGamePaused = true;
        GameState.isMoveMap = false;

        if (popupEnough != null)
            popupEnough.SetActive(false);

        countdownValue = StartCountdownValue;
        UpdateCountdownText();

        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
        countdownCoroutine = StartCoroutine(CountdownRoutine());
    }

    private void OnDisable()
    {
        RemoveListener(saveButton, ClickSave);
        RemoveListener(noButton, ClickNo);
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
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

    private IEnumerator CountdownRoutine()
    {
        while (countdownValue > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            countdownValue--;
            UpdateCountdownText();
        }
        ClickNo();
    }

    public void ClickSave()
    {
        PlayClickSound();

        int cost = DefaultSaveCost;
        if (costText != null && int.TryParse(costText.text, out int parsedCost))
        {
            cost = parsedCost;
        }

        if (GameState.GetCrystal() >= cost)
        {
            GameState.AddCrystal(-cost);
            GameState.SaveCrystal();

            if (GamePlayEventListener.instance != null)
            {
                GamePlayEventListener.instance.ReviveFromSaveMe();
            }
            else
            {
                gameObject.SetActive(false);
                if (MyPlaneController.instance != null)
                {
                    MyPlaneController.instance.gameObject.SetActive(true);
                    MyPlaneController.instance.energy = GameState.energyPlane;
                }
                Time.timeScale = 1f;
                GameState.isGamePaused = false;
                GameState.isMoveMap = true;
            }
        }
        else
        {
            if (popupEnough != null)
            {
                popupEnough.SetActive(true);
                StartCoroutine(HidePopupEnoughRoutine());
            }
        }
    }

    private IEnumerator HidePopupEnoughRoutine()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (popupEnough != null)
        {
            popupEnough.SetActive(false);
        }
    }

    public void ClickNo()
    {
        PlayClickSound();

        gameObject.SetActive(false);
        GameState.lose = true;
        Time.timeScale = 1f;
    }

    private void UpdateCountdownText()
    {
        if (countdownText != null)
            countdownText.text = countdownValue.ToString("00");
    }

    private void PlayClickSound()
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
