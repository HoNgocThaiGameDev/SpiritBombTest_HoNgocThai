using UnityEngine;

public static class Vibration
{
    private const string VibratorService = "vibrator";
    private const int AndroidApiOreo = 26;
    private const int MinAmplitude = 1;
    private const int MaxAmplitude = 255;

    private static AndroidJavaObject vibrator;

    public static void Vibrate()
    {
        Vibrate(80);
    }

    public static void Vibrate(long milliseconds)
    {
        Vibrate(milliseconds, -1);
    }

    public static void Vibrate(long milliseconds, int amplitude)
    {
        if (milliseconds <= 0 || !HasVibrator())
            return;

        try
        {
            AndroidJavaObject androidVibrator = GetAndroidVibrator();
            if (androidVibrator == null)
                return;

            if (GetAndroidSdkVersion() >= AndroidApiOreo)
            {
                using (AndroidJavaClass vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect"))
                {
                    int safeAmplitude = GetSafeAmplitude(vibrationEffect, amplitude);
                    using (AndroidJavaObject effect = vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, safeAmplitude))
                    {
                        androidVibrator.Call("vibrate", effect);
                    }
                }
            }
            else
            {
                androidVibrator.Call("vibrate", milliseconds);
            }
        }
        catch (AndroidJavaException exception)
        {
            Debug.LogWarning("[Vibration] Android vibration failed: " + exception.Message);
            Handheld.Vibrate();
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (pattern == null || pattern.Length == 0 || !HasVibrator())
            return;

        try
        {
            AndroidJavaObject androidVibrator = GetAndroidVibrator();
            if (androidVibrator == null)
                return;

            if (GetAndroidSdkVersion() >= AndroidApiOreo)
            {
                using (AndroidJavaClass vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect"))
                using (AndroidJavaObject effect = vibrationEffect.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat))
                {
                    androidVibrator.Call("vibrate", effect);
                }
            }
            else
            {
                androidVibrator.Call("vibrate", pattern, repeat);
            }
        }
        catch (AndroidJavaException exception)
        {
            Debug.LogWarning("[Vibration] Android vibration pattern failed: " + exception.Message);
            Handheld.Vibrate();
        }
    }

    public static bool HasVibrator()
    {
        if (Application.platform != RuntimePlatform.Android)
            return false;

        try
        {
            AndroidJavaObject androidVibrator = GetAndroidVibrator();
            return androidVibrator != null && androidVibrator.Call<bool>("hasVibrator");
        }
        catch (AndroidJavaException exception)
        {
            Debug.LogWarning("[Vibration] Could not check Android vibrator: " + exception.Message);
            return false;
        }
    }

    public static void Cancel()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        try
        {
            AndroidJavaObject androidVibrator = GetAndroidVibrator();
            if (androidVibrator != null)
                androidVibrator.Call("cancel");
        }
        catch (AndroidJavaException exception)
        {
            Debug.LogWarning("[Vibration] Could not cancel Android vibration: " + exception.Message);
        }
    }

    private static AndroidJavaObject GetAndroidVibrator()
    {
        if (Application.platform != RuntimePlatform.Android)
            return null;

        if (vibrator != null)
            return vibrator;

        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            if (activity == null)
                return null;

            vibrator = activity.Call<AndroidJavaObject>("getSystemService", VibratorService);
            return vibrator;
        }
    }

    private static int GetAndroidSdkVersion()
    {
        using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
    }

    private static int GetSafeAmplitude(AndroidJavaClass vibrationEffect, int amplitude)
    {
        if (amplitude < 0)
            return vibrationEffect.GetStatic<int>("DEFAULT_AMPLITUDE");

        return Mathf.Clamp(amplitude, MinAmplitude, MaxAmplitude);
    }
}
