using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelControl : MonoBehaviour
{
    public Button closeButton;

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
    }

    private void OnDisable()
    {
        UnregisterListeners();
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
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

    private void RefreshOptionIcons()
    {
        RefreshSoundState();
        RefreshVibrationState();
    }

    private void RefreshSoundState()
    {
        SetStateObjects(soundOnState, soundOffState, PlayerSettingsService.IsSoundEnabled());
    }

    private void RefreshVibrationState()
    {
        SetStateObjects(vibrationOnState, vibrationOffState, PlayerSettingsService.IsVibrationEnabled());
    }

    private void RegisterListeners()
    {
        AddButtonListener(closeButton, ClosePanel);
        AddStateListener(soundOnState, DisableSound);
        AddStateListener(soundOffState, EnableSound);
        AddStateListener(vibrationOnState, DisableVibration);
        AddStateListener(vibrationOffState, EnableVibration);
    }

    private void UnregisterListeners()
    {
        RemoveButtonListener(closeButton, ClosePanel);
        RemoveStateListener(soundOnState, DisableSound);
        RemoveStateListener(soundOffState, EnableSound);
        RemoveStateListener(vibrationOnState, DisableVibration);
        RemoveStateListener(vibrationOffState, EnableVibration);
    }

    private static void SetStateObjects(GameObject onObject, GameObject offObject, bool enabled)
    {
        if (onObject != null)
            onObject.SetActive(enabled);
        if (offObject != null)
            offObject.SetActive(!enabled);
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

    private static void AddStateListener(GameObject target, UnityEngine.Events.UnityAction action)
    {
        if (target == null)
            return;

        Button button = target.GetComponent<Button>();
        if (button == null)
            button = target.AddComponent<Button>();

        AddButtonListener(button, action);
    }

    private static void RemoveStateListener(GameObject target, UnityEngine.Events.UnityAction action)
    {
        if (target == null)
            return;

        Button button = target.GetComponent<Button>();
        RemoveButtonListener(button, action);
    }
}
