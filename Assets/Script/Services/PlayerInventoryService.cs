using UnityEngine;

public static class PlayerInventoryService
{
    public static int GetEnergy()
    {
        return ProfileManager.settingProfile.energyEncryption;
    }

    public static void SetEnergy(int value)
    {
        ProfileManager.settingProfile.energyEncryption.Set(Mathf.Max(0, value));
    }

    public static int GetGold()
    {
        return ProfileManager.settingProfile.goldEncryption;
    }

    public static void SetGold(int value)
    {
        ProfileManager.settingProfile.goldEncryption.Set(Mathf.Max(0, value));
    }

    public static int GetCrystal()
    {
        return ProfileManager.settingProfile.crystalEncryption;
    }

    public static void SetCrystal(int value)
    {
        ProfileManager.settingProfile.crystalEncryption.Set(Mathf.Max(0, value));
    }

    public static int GetRocket()
    {
        return ProfileManager.settingProfile.rocketEncryption;
    }

    public static void SetRocket(int value)
    {
        ProfileManager.settingProfile.rocketEncryption.Set(Mathf.Max(0, value));
    }

    public static int GetSupport()
    {
        return ProfileManager.settingProfile.supportEncryption;
    }

    public static void SetSupport(int value)
    {
        ProfileManager.settingProfile.supportEncryption.Set(Mathf.Max(0, value));
    }

    public static int GetShield()
    {
        return ProfileManager.settingProfile.shieldEncryption;
    }

    public static void SetShield(int value)
    {
        ProfileManager.settingProfile.shieldEncryption.Set(Mathf.Max(0, value));
    }

    public static void AddCoin(int amount, ref SecuredInt runCoin)
    {
        runCoin.Value += amount;
    }

    public static void AddCrystal(int amount, ref SecuredInt runCrystal)
    {
        runCrystal.Value += amount;
    }

    public static void AddEnergy(int amount, ref SecuredInt runEnergy)
    {
        runEnergy.Value = Mathf.Max(0, runEnergy.Value + amount);
    }

    public static void SaveCoin(int value)
    {
        SetGold(value);
    }

    public static void SaveCrystal(int value)
    {
        SetCrystal(value);
    }

    public static void SaveEnergy(int value)
    {
        SetEnergy(value);
    }

    public static void SaveItem(int levelNumber, int rocket, int shield, int support)
    {
        if (levelNumber == 1)
            return;

        SetRocket(rocket);
        SetShield(shield);
        SetSupport(support);
    }

}
