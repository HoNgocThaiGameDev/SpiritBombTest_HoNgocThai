using UnityEngine;

public class GamePlayerData : ISaveObject
{
    private GamePlayerDataSave save;

    public FloatProfileData soundVolume { get; set; }
    public FloatProfileData musicVolume { get; set; }
    public FloatProfileData buttonTransparency { get; set; }
    public BoolProfileData isDynamicJoystick { get; set; }
    public IntProfileData[] highScoreEncryption { get; set; }
    public IntProfileData[] starLevelEncryption { get; set; }
    public IntProfileData[] lockPlaneEncryption { get; set; }
    public IntProfileData energyEncryption { get; set; }
    public IntProfileData goldEncryption { get; set; }
    public IntProfileData crystalEncryption { get; set; }
    public IntProfileData rocketEncryption { get; set; }
    public IntProfileData shieldEncryption { get; set; }
    public IntProfileData supportEncryption { get; set; }
    public IntProfileData levelCompletedEncryption { get; set; }
    public IntProfileData[] levelPlaneEncryption { get; set; }
    public IntProfileData levelMissile { get; set; }
    public IntProfileData levelSupport { get; set; }
    public IntProfileData levelShield { get; set; }
    public IntProfileData[] levelTierPlane { get; set; }
    public IntProfileData planeUpgrade { get; set; }
    public IntProfileData timeUpgrade { get; set; }
    public StringProfileData namePlaneUpgrade { get; set; }
    public IntProfileData sound { get; set; }
    public IntProfileData vibration { get; set; }
    public IntProfileData control { get; set; }
    public FloatProfileData quitTime { get; set; }

    public GamePlayerData()
    {
    }

    public GamePlayerData(GamePlayerDataSave save)
    {
        this.save = save;
        BindProperties();
    }

    public void Flush()
    {
        if (save != null)
        {
            save.Flush();
        }
    }

    public void BindProperties()
    {
        if (save == null)
        {
            save = new GamePlayerDataSave();
        }
        save.InitializeDefaultValues(false);

        soundVolume = new FloatProfileData(() => save.soundVolume, v => save.soundVolume = v, 1f);
        musicVolume = new FloatProfileData(() => save.musicVolume, v => save.musicVolume = v, 0.5f);
        buttonTransparency = new FloatProfileData(() => save.buttonTransparency, v => save.buttonTransparency = v, 1f);
        isDynamicJoystick = new BoolProfileData(() => save.isDynamicJoystick, v => save.isDynamicJoystick = v, false);

        highScoreEncryption = new IntProfileData[GamePlayerDataSave.LevelCount];
        for (int i = 0; i < GamePlayerDataSave.LevelCount; i++)
        {
            int index = i;
            highScoreEncryption[i] = new IntProfileData(() => save.highScore[index], v => save.highScore[index] = v, 0);
        }

        starLevelEncryption = new IntProfileData[GamePlayerDataSave.LevelCount];
        for (int i = 0; i < GamePlayerDataSave.LevelCount; i++)
        {
            int index = i;
            starLevelEncryption[i] = new IntProfileData(() => save.starLevel[index], v => save.starLevel[index] = v, 0);
        }

        lockPlaneEncryption = new IntProfileData[GamePlayerDataSave.PlayerPlaneCount];
        for (int i = 0; i < GamePlayerDataSave.PlayerPlaneCount; i++)
        {
            int index = i;
            int defaultVal = (index == 0) ? 1 : 0;
            lockPlaneEncryption[i] = new IntProfileData(() => save.lockPlane[index], v => save.lockPlane[index] = v, defaultVal);
        }

        energyEncryption = new IntProfileData(() => save.energy, v => save.energy = v, 50);
        goldEncryption = new IntProfileData(() => save.gold, v => save.gold = v, 0);
        crystalEncryption = new IntProfileData(() => save.crystal, v => save.crystal = v, 0);
        rocketEncryption = new IntProfileData(() => save.rocket, v => save.rocket = v, 3);
        shieldEncryption = new IntProfileData(() => save.shield, v => save.shield = v, 3);
        supportEncryption = new IntProfileData(() => save.support, v => save.support = v, 3);
        levelCompletedEncryption = new IntProfileData(() => save.levelCompleted, v => save.levelCompleted = v, 0);

        levelPlaneEncryption = new IntProfileData[GamePlayerDataSave.PlayerPlaneCount];
        for (int i = 0; i < GamePlayerDataSave.PlayerPlaneCount; i++)
        {
            int index = i;
            levelPlaneEncryption[i] = new IntProfileData(() => save.levelPlane[index], v => save.levelPlane[index] = v, 1);
        }

        levelTierPlane = new IntProfileData[GamePlayerDataSave.PlayerPlaneCount];
        for (int i = 0; i < GamePlayerDataSave.PlayerPlaneCount; i++)
        {
            int index = i;
            levelTierPlane[i] = new IntProfileData(() => save.levelTierPlane[index], v => save.levelTierPlane[index] = v, 0);
        }

        levelMissile = new IntProfileData(() => save.levelMissile, v => save.levelMissile = v, 1);
        levelSupport = new IntProfileData(() => save.levelSupport, v => save.levelSupport = v, 1);
        levelShield = new IntProfileData(() => save.levelShield, v => save.levelShield = v, 1);

        planeUpgrade = new IntProfileData(() => save.planeUpgrade, v => save.planeUpgrade = v, 0);
        timeUpgrade = new IntProfileData(() => save.timeUpgrade, v => save.timeUpgrade = v, 0);
        namePlaneUpgrade = new StringProfileData(() => save.namePlaneUpgrade, v => save.namePlaneUpgrade = v, string.Empty);
        sound = new IntProfileData(() => save.sound, v => save.sound = v, 1);
        vibration = new IntProfileData(() => save.vibration, v => save.vibration = v, 0);
        control = new IntProfileData(() => save.control, v => save.control = v, 1);
        quitTime = new FloatProfileData(() => save.quitTime, v => save.quitTime = v, 0f);
    }
}
