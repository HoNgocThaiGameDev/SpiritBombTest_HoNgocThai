using UnityEngine;
using UnityEngine.UI;

public class PauseControll : MonoBehaviour
{
    public Button close;
    public Button resume;
    public Button retry;
    public Button quit;
    public Button yes;
    public Button no;
    public GameObject quitPanel;
    [Header("Sound Toggle State")]
    public GameObject soundOnState;
    public GameObject soundOffState;
    [Header("Vibration Toggle State")]
    public GameObject vibrationOnState;
    public GameObject vibrationOffState;
    private void OnEnable()
    {
        RegisterListeners();
        RefreshOptionIcons();
        GameState.isMoveMap = false;
    }

    private void OnDisable()
    {
        UnregisterListeners();
        GameState.isMoveMap = true;
        if (quitPanel != null)
            quitPanel.SetActive(false);
    }

    private void RefreshOptionIcons()
    {
        RefreshSoundState();
        RefreshVibrationState();

    }

    private void CloseClick()
    {
        if (quitPanel != null)
            quitPanel.SetActive(false);
        gameObject.SetActive(false);
        if (Time.timeScale == 0)
        {
            ResumeGameAudio();
            GameState.isGamePaused = false;
        }
    }

    private void ResumeClick()
    {
        ResumeGameAudio();
        gameObject.SetActive(false);
        GameState.isGamePaused = false;
    }

    private void RetryClick()
    {
        GamePlayEventListener.instance.RightCanvas.SetActive(false);
        SceneFlowService.RetryGameplay();
        Time.timeScale = 1;
        if (AudioListener.pause && PlayerSettingsService.IsSoundEnabled())
            AudioListener.pause = false;
        gameObject.SetActive(false);
    }

    private void QuitClick()
    {
        quitPanel.SetActive(true);
        AudioListener.pause = true;
        GameState.isGamePaused = true;
    }

    public void soundClick()
    {
        ToggleSound();
    }

    public void vibrationClick()
    {
        ToggleVibration();
    }

    private void ToggleSound()
    {
        SetSoundEnabled(!PlayerSettingsService.IsSoundEnabled());
    }

    private void ToggleVibration()
    {
        SetVibrationEnabled(!PlayerSettingsService.IsVibrationEnabled());
    }

    private void EnableSound()
    {
        SetSoundEnabled(true);
    }

    private void DisableSound()
    {
        SetSoundEnabled(false);
    }

    private void EnableVibration()
    {
        SetVibrationEnabled(true);
    }

    private void DisableVibration()
    {
        SetVibrationEnabled(false);
    }

    private void SetSoundEnabled(bool enabled)
    {
        PlayerSettingsService.SetSound(enabled ? 1 : 0);
        AudioListener.pause = !enabled;
        RefreshSoundState();
    }

    private void SetVibrationEnabled(bool enabled)
    {
        PlayerSettingsService.SetVibration(enabled ? 1 : 0);
        RefreshVibrationState();
    }

    private void RefreshSoundState()
    {
        bool enabled = PlayerSettingsService.IsSoundEnabled();
        SetStateObjects(soundOnState, soundOffState, enabled);
    }

    private void RefreshVibrationState()
    {
        bool enabled = PlayerSettingsService.IsVibrationEnabled();
        SetStateObjects(vibrationOnState, vibrationOffState, enabled);
    }

    private static void SetStateObjects(GameObject onObject, GameObject offObject, bool enabled)
    {
        if (onObject != null)
            onObject.SetActive(enabled);
        if (offObject != null)
            offObject.SetActive(!enabled);
    }

    private static void AddClickListener(GameObject target, UnityEngine.Events.UnityAction action)
    {
        if (target == null)
            return;

        Button button = target.GetComponent<Button>();
        if (button == null)
            button = target.AddComponent<Button>();
        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    private void RegisterListeners()
    {
        AddButtonListener(close, CloseClick);
        AddButtonListener(resume, ResumeClick);
        AddButtonListener(retry, RetryClick);
        AddButtonListener(quit, QuitClick);
        AddButtonListener(yes, YesClick);
        AddButtonListener(no, NoClick);
        AddClickListener(soundOnState, DisableSound);
        AddClickListener(soundOffState, EnableSound);
        AddClickListener(vibrationOnState, DisableVibration);
        AddClickListener(vibrationOffState, EnableVibration);
    }

    private void UnregisterListeners()
    {
        RemoveButtonListener(close, CloseClick);
        RemoveButtonListener(resume, ResumeClick);
        RemoveButtonListener(retry, RetryClick);
        RemoveButtonListener(quit, QuitClick);
        RemoveButtonListener(yes, YesClick);
        RemoveButtonListener(no, NoClick);
        RemoveClickListener(soundOnState, DisableSound);
        RemoveClickListener(soundOffState, EnableSound);
        RemoveClickListener(vibrationOnState, DisableVibration);
        RemoveClickListener(vibrationOffState, EnableVibration);
    }

    private static void AddButtonListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null)
            return;

        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    private static void RemoveButtonListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.RemoveListener(action);
    }

    private static void RemoveClickListener(GameObject target, UnityEngine.Events.UnityAction action)
    {
        if (target == null)
            return;

        Button button = target.GetComponent<Button>();
        if (button != null)
            button.onClick.RemoveListener(action);
    }

    private void YesClick()
    {
        GameState.ResetAll();
        GameState.SaveCoin();
        GameState.SaveCrystal();
        GameState.SaveEnergy();
        GameState.SaveItem();
        GamePlayEventListener.instance.RightCanvas.SetActive(false);
        Time.timeScale = 1;
        if (PlayerSettingsService.IsSoundEnabled())
            AudioListener.pause = false;
        if (SoundController.instance != null)
        {
            SoundController.instance.StopAllSound();
            SoundController.instance.PlaySound(8);
        }
        SceneFlowService.LoadMenu(false);
    }

    private void NoClick()
    {
        ResumeGameAudio();
        quitPanel.SetActive(false);
        gameObject.SetActive(false);
        GameState.isGamePaused = false;
    }

    private static void ResumeGameAudio()
    {
        Time.timeScale = 1;
        if (PlayerSettingsService.IsSoundEnabled())
            AudioListener.pause = false;
    }
}
