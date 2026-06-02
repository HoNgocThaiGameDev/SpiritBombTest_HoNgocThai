using UnityEngine;

public static class PlayerUpgradeService
{
    public const int DefaultPlaneIndex = 0;
    private const int MaxTrackedPlanes = GamePlayerDataSave.PlayerPlaneCount;

    public static int GetPlaneUpgrading()
    {
        return ProfileManager.settingProfile.planeUpgrade;
    }

    public static void SetPlaneUpgrading(int value)
    {
        ProfileManager.settingProfile.planeUpgrade.Set(Mathf.Max(0, value));
        if (value == 0)
        {
            SetTimeUpgrade(0);
            SetNamePlaneUpgrade(string.Empty);
        }
    }

    public static int GetTimeUpgrade()
    {
        return ProfileManager.settingProfile.timeUpgrade;
    }

    public static void SetTimeUpgrade(int value)
    {
        ProfileManager.settingProfile.timeUpgrade.Set(Mathf.Max(0, value));
    }

    public static string GetNamePlaneUpgrade()
    {
        return ProfileManager.settingProfile.namePlaneUpgrade;
    }

    public static void SetNamePlaneUpgrade(string value)
    {
        ProfileManager.settingProfile.namePlaneUpgrade.Set(value ?? string.Empty);
    }

    public static int GetLevelPlane(int index)
    {
        if (!IsValidPlaneIndex(index, "GetLevelPlane"))
            return 1;

        return ProfileManager.settingProfile.levelPlaneEncryption[index];
    }

    public static void SetLevelPlane(int index, int value)
    {
        if (!IsValidPlaneIndex(index, "SetLevelPlane"))
            return;

        ProfileManager.settingProfile.levelPlaneEncryption[index].Set(Mathf.Max(1, value));
    }

    public static int GetLevelRankPlane(int index)
    {
        if (!IsValidPlaneIndex(index, "GetLevelRankPlane"))
            return 0;

        return ProfileManager.settingProfile.levelTierPlane[index];
    }

    public static void SetLevelRankPlane(int index, int value)
    {
        if (!IsValidPlaneIndex(index, "SetLevelRankPlane"))
            return;

        ProfileManager.settingProfile.levelTierPlane[index].Set(Mathf.Max(0, value));
    }

    public static int GetLockPlane(int index)
    {
        return 1;
    }

    public static void SetLockPlane(int index, int value)
    {
        if (!IsValidPlaneIndex(index, "SetLockPlane"))
            return;

        ProfileManager.settingProfile.lockPlaneEncryption[index].Set(value);
    }

    public static int GetLevelMissile()
    {
        return ProfileManager.settingProfile.levelMissile;
    }

    public static void SetLevelMissile(int level)
    {
        ProfileManager.settingProfile.levelMissile.Set(Mathf.Max(1, level));
    }

    public static int GetLevelSupport()
    {
        return ProfileManager.settingProfile.levelSupport;
    }

    public static void SetLevelSupport(int level)
    {
        ProfileManager.settingProfile.levelSupport.Set(Mathf.Max(1, level));
    }

    public static int GetLevelShield()
    {
        return ProfileManager.settingProfile.levelShield;
    }

    public static void SetLevelShield(int level)
    {
        ProfileManager.settingProfile.levelShield.Set(Mathf.Max(1, level));
    }

    private static bool IsValidPlaneIndex(int index, string caller)
    {
        if (index >= 0 && index < MaxTrackedPlanes)
            return true;

        Debug.LogWarning("[PlayerUpgradeService] " + caller + " ignored invalid plane index: " + index);
        return false;
    }
}
