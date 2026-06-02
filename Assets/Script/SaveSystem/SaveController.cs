using UnityEngine;

public static class SaveController
{
    private const string SAVE_KEY = "Test_SpiritBomb";
    private const int CurrentSaveSchemaVersion = 5;
    private static GamePlayerDataSave saveData;
    private static bool isSaveLoaded;
    private static bool isSaveRequired;

    public static void Initialise(bool clearSave = false)
    {
        if (clearSave)
        {
            saveData = CreateDefaultSave();
            isSaveLoaded = true;
            Save(forceSave: true);
            return;
        }

        Load();
    }

    private static void Load()
    {
        if (isSaveLoaded) return;

        bool isNewSave = !PlayerPrefs.HasKey(SAVE_KEY);
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            try
            {
                saveData = JsonUtility.FromJson<GamePlayerDataSave>(json);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[SaveController] Failed to parse save: " + ex.Message);
                saveData = CreateDefaultSave();
                isNewSave = true;
            }
        }
        else
        {
            saveData = CreateDefaultSave();
        }

        if (saveData == null)
        {
            saveData = new GamePlayerDataSave();
            isNewSave = true;
        }

        saveData.InitializeDefaultValues(isNewSave);
        if (isNewSave)
        {
            isSaveRequired = true;
        }

        MigrateLegacyPlayerPrefs(saveData);
        isSaveLoaded = true;
        if (isSaveRequired)
        {
            Save(forceSave: true);
        }
    }

    public static void Save(bool forceSave = false)
    {
        if (!forceSave && !isSaveRequired) return;
        if (saveData == null)
            saveData = CreateDefaultSave();

        saveData.Flush();
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        isSaveRequired = false;
    }

    public static GamePlayerDataSave GetSaveObject()
    {
        if (!isSaveLoaded)
        {
            Initialise();
        }
        return saveData;
    }

    public static void MarkAsSaveIsRequired()
    {
        isSaveRequired = true;
        Save();
    }

    private static GamePlayerDataSave CreateDefaultSave()
    {
        GamePlayerDataSave data = new GamePlayerDataSave();
        data.InitializeDefaultValues(true);
        data.saveSchemaVersion = CurrentSaveSchemaVersion;
        return data;
    }

    private static void MigrateLegacyPlayerPrefs(GamePlayerDataSave data)
    {
        if (data.saveSchemaVersion >= CurrentSaveSchemaVersion)
            return;

        data.planeUpgrade = PlayerPrefs.GetInt("PlaneUpgrade", data.planeUpgrade);
        data.timeUpgrade = PlayerPrefs.GetInt("TimeUpgrade", data.timeUpgrade);
        data.namePlaneUpgrade = PlayerPrefs.GetString("NamePlaneUpgrade", data.namePlaneUpgrade ?? string.Empty);
        data.sound = PlayerPrefs.GetInt("sound", data.sound == 0 ? 1 : data.sound);
        data.vibration = PlayerPrefs.GetInt("Vibration", data.vibration);
        data.control = PlayerPrefs.GetInt("Control", data.control == 0 ? 1 : data.control);
        data.quitTime = PlayerPrefs.GetFloat(StringManager.PlayerPrefKeys.quitTime, data.quitTime);
        data.saveSchemaVersion = CurrentSaveSchemaVersion;
        isSaveRequired = true;
    }
}
