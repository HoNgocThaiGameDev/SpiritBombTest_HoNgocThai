using UnityEngine;
using System;

[Serializable]
public class GamePlayerDataSave : ISaveObject
{
    public const int LevelCount = 3;
    public const int PlayerPlaneCount = 1;
    private const float DefaultSoundVolume = 1f;
    private const float DefaultMusicVolume = 0.5f;
    private const float DefaultButtonTransparency = 1f;
    private const int FirstRunEnergy = 99999;
    private const int FirstRunGold = 99999;
    private const int FirstRunCrystal = 99999;
    private const int FirstRunBoostCount = 3;
    private const int FirstRunPlaneLevel = 1;
    private const int FirstRunUpgradeLevel = 1;
    private const int FirstRunSound = 1;
    private const int FirstRunVibration = 1;
    private const int FirstRunControl = 1;
    private const float FirstRunQuitTime = 1780388331520f;

    public float soundVolume;
    public float musicVolume;
    public float buttonTransparency;
    public bool isDynamicJoystick;
    public int[] highScore;
    public int[] starLevel;
    public int[] lockPlane;
    public int energy;
    public int gold;
    public int crystal;
    public int rocket;
    public int shield;
    public int support;
    public int levelCompleted;
    public int[] levelPlane;
    public int[] levelTierPlane;
    public int levelMissile;
    public int levelSupport;
    public int levelShield;
    public int planeUpgrade;
    public int timeUpgrade;
    public string namePlaneUpgrade;
    public int sound;
    public int vibration;
    public int control;
    public float quitTime;
    public int saveSchemaVersion;

    public void Flush()
    {
    }

    public void InitializeDefaultValues(bool isNewSave)
    {
        if (isNewSave)
        {
            ApplyFirstRunDefaults();
        }

        if (soundVolume == 0f)
            soundVolume = DefaultSoundVolume;

        if (musicVolume == 0f)
            musicVolume = DefaultMusicVolume;

        if (buttonTransparency == 0f)
            buttonTransparency = DefaultButtonTransparency;

        highScore = ResizeArray(highScore, LevelCount, 0);
        starLevel = ResizeArray(starLevel, LevelCount, 0);
        lockPlane = ResizeArray(lockPlane, PlayerPlaneCount, 0);
        lockPlane[0] = 1;
        levelCompleted = Math.Min(Math.Max(levelCompleted, 0), LevelCount);

        levelPlane = ResizeArray(levelPlane, PlayerPlaneCount, 1);
        levelTierPlane = ResizeArray(levelTierPlane, PlayerPlaneCount, 0);

        if (levelMissile == 0)
            levelMissile = FirstRunUpgradeLevel;

        if (levelSupport == 0)
            levelSupport = FirstRunUpgradeLevel;

        if (levelShield == 0)
            levelShield = FirstRunUpgradeLevel;

        if (namePlaneUpgrade == null)
            namePlaneUpgrade = string.Empty;
    }

    private void ApplyFirstRunDefaults()
    {
        soundVolume = DefaultSoundVolume;
        musicVolume = DefaultMusicVolume;
        buttonTransparency = DefaultButtonTransparency;
        isDynamicJoystick = false;
        highScore = CreateArray(LevelCount, 0);
        starLevel = CreateArray(LevelCount, 0);
        lockPlane = CreateArray(PlayerPlaneCount, 1);
        energy = FirstRunEnergy;
        gold = FirstRunGold;
        crystal = FirstRunCrystal;
        rocket = FirstRunBoostCount;
        shield = FirstRunBoostCount;
        support = FirstRunBoostCount;
        levelCompleted = 0;
        levelPlane = CreateArray(PlayerPlaneCount, FirstRunPlaneLevel);
        levelTierPlane = CreateArray(PlayerPlaneCount, 0);
        levelMissile = FirstRunUpgradeLevel;
        levelSupport = FirstRunUpgradeLevel;
        levelShield = FirstRunUpgradeLevel;
        planeUpgrade = 0;
        timeUpgrade = 0;
        namePlaneUpgrade = string.Empty;
        sound = FirstRunSound;
        vibration = FirstRunVibration;
        control = FirstRunControl;
        quitTime = FirstRunQuitTime;
    }

    private static int[] CreateArray(int length, int value)
    {
        int[] result = new int[length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = value;
        }

        return result;
    }

    private static int[] ResizeArray(int[] source, int length, int defaultValue)
    {
        int[] result = new int[length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = defaultValue;
        }

        if (source != null)
        {
            Array.Copy(source, result, Math.Min(source.Length, result.Length));
        }

        return result;
    }

}
