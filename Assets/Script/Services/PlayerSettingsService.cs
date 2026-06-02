using UnityEngine;

public static class PlayerSettingsService
{
    public static bool IsSoundEnabled()
    {
        return GetSound() == 1;
    }

    public static int GetSound()
    {
        return ProfileManager.settingProfile.sound;
    }

    public static void SetSound(int value)
    {
        ProfileManager.settingProfile.sound.Set(value == 0 ? 0 : 1);
    }

    public static bool IsVibrationEnabled()
    {
        return GetVibration() == 1;
    }

    public static int GetVibration()
    {
        return ProfileManager.settingProfile.vibration;
    }

    public static void SetVibration(int value)
    {
        ProfileManager.settingProfile.vibration.Set(value == 0 ? 0 : 1);
    }

    public static int GetControl()
    {
        int control = ProfileManager.settingProfile.control;
        return control == 2 ? 2 : 1;
    }

    public static void SetControl(int value)
    {
        ProfileManager.settingProfile.control.Set(value == 2 ? 2 : 1);
    }

    public static float GetQuitTime()
    {
        return ProfileManager.settingProfile.quitTime;
    }

    public static void SetQuitTime(float value)
    {
        ProfileManager.settingProfile.quitTime.Set(Mathf.Max(0f, value));
    }

}
