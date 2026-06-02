using UnityEngine;

public static class PlayerProgressService
{
    public const int MaxTrackedLevels = GamePlayerDataSave.LevelCount;

    public static int GetLevelCompleted()
    {
        return ProfileManager.settingProfile.levelCompletedEncryption;
    }

    public static void SetLevelCompleted(int value)
    {
        ProfileManager.settingProfile.levelCompletedEncryption.Set(Mathf.Clamp(value, 0, MaxTrackedLevels));
    }

    public static int GetLevelStar(int index)
    {
        if (!IsValidLevelIndex(index, "GetLevelStar"))
            return 0;

        return ProfileManager.settingProfile.starLevelEncryption[index];
    }

    public static void SetLevelStar(int index, int value)
    {
        if (!IsValidLevelIndex(index, "SetLevelStar"))
            return;

        ProfileManager.settingProfile.starLevelEncryption[index].Set(Mathf.Max(0, value));
    }

    public static int GetHighScore(int index)
    {
        if (!IsValidLevelIndex(index, "GetHighScore"))
            return 0;

        return ProfileManager.settingProfile.highScoreEncryption[index];
    }

    public static void SetHighScore(int index, int score)
    {
        if (!IsValidLevelIndex(index, "SetHighScore"))
            return;

        ProfileManager.settingProfile.highScoreEncryption[index].Set(Mathf.Max(0, score));
    }

    public static int GetTotalHighScore()
    {
        return CalculateTotalHighScore();
    }

    public static void SaveLevelComplete(int levelNumber, int star)
    {
        if (levelNumber < 1)
            return;

        if (GetLevelCompleted() <= levelNumber)
            SetLevelCompleted(levelNumber);

        int starIndex = levelNumber - 1;
        if (GetLevelStar(starIndex) < star)
            SetLevelStar(starIndex, star);
    }

    public static int GetStar()
    {
        return GetAllStarLevel(GetLevelCompleted());
    }

    public static int GetAllStarLevel(int level)
    {
        int total = 0;
        int cappedLevel = Mathf.Clamp(level, 0, MaxTrackedLevels);
        for (int i = 1; i <= cappedLevel; i++)
        {
            total += GetLevelStar(i - 1);
        }

        return total;
    }

    public static void SaveHighScore(int levelNumber, int score)
    {
        int levelIndex = levelNumber - 1;
        if (!IsValidLevelIndex(levelIndex, "SaveHighScore"))
            return;

        if (score > GetHighScore(levelIndex))
            SetHighScore(levelIndex, score);
    }

    private static int CalculateTotalHighScore()
    {
        int totalHighScore = 0;
        for (int i = 0; i < MaxTrackedLevels; i++)
        {
            totalHighScore += GetHighScore(i);
        }

        return totalHighScore;
    }

    private static bool IsValidLevelIndex(int index, string caller)
    {
        if (index >= 0 && index < MaxTrackedLevels)
            return true;

        Debug.LogWarning("[PlayerProgressService] " + caller + " ignored invalid level index: " + index);
        return false;
    }
}
