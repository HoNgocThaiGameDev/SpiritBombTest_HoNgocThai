using UnityEngine;

public class ProfileManager
{
    public static GamePlayerData playerData;

    public static GamePlayerData settingProfile
    {
        get
        {
            if (playerData == null)
            {
                Init();
            }
            return playerData;
        }
    }

    public static void Init()
    {
        if (playerData == null)
        {
            SaveController.Initialise();
            playerData = new GamePlayerData(SaveController.GetSaveObject());
        }
    }

    public static void Load()
    {
        if (playerData == null)
        {
            playerData = new GamePlayerData(SaveController.GetSaveObject());
        }
    }

    public static void SaveAll()
    {
        SaveController.Save(forceSave: true);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        SaveController.Initialise(clearSave: true);
        playerData = new GamePlayerData(SaveController.GetSaveObject());
        SaveController.Save(forceSave: true);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RuntimeInit()
    {
        Init();
    }
}
