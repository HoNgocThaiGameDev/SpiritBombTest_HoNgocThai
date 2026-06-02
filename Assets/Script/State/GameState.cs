using UnityEngine;

public static class GameState
{
    public const int MAX_ENERGY_REGEN = 100;
    public const int ENERGY_REGEN_INTERVAL = 180;
    public const string NUMBER_BUY_ENERGRY = "NumberBuyEnergy";

    public static bool LevelFull { get { return GameSessionState.LevelFull; } set { GameSessionState.LevelFull = value; } }
    public static bool PlaneFull { get { return GameSessionState.PlaneFull; } set { GameSessionState.PlaneFull = value; } }
    public static bool isShowAnim { get { return GameSessionState.IsShowAnim; } set { GameSessionState.IsShowAnim = value; } }
    public static bool isX2Reward { get { return GameSessionState.IsX2Reward; } set { GameSessionState.IsX2Reward = value; } }
    public static bool isSaveMe { get { return GameSessionState.IsSaveMe; } set { GameSessionState.IsSaveMe = value; } }
    public static bool isReduce10Percent { get { return GameSessionState.IsReduce10Percent; } set { GameSessionState.IsReduce10Percent = value; } }
    public static bool isRestartFire { get { return GameSessionState.IsRestartFire; } set { GameSessionState.IsRestartFire = value; } }
    public static string language { get { return GameSessionState.Language; } set { GameSessionState.Language = value; } }
    public static int currentLevelPlane { get { return GameSessionState.CurrentLevelPlane; } set { GameSessionState.CurrentLevelPlane = value; } }
    public static int countWin { get { return GameSessionState.CountWin; } set { GameSessionState.CountWin = value; } }

    public static SecuredInt levelMissile = new SecuredInt(1);
    public static SecuredInt levelSupport = new SecuredInt(1);
    public static SecuredInt levelShield = new SecuredInt(1);

    public static string[] listTip { get { return GameSessionState.ListTip; } set { GameSessionState.ListTip = value; } }
    public static string[] listSlogan { get { return GameSessionState.ListSlogan; } set { GameSessionState.ListSlogan = value; } }
    public static string[] listMapName { get { return GameSessionState.ListMapName; } set { GameSessionState.ListMapName = value; } }

    public static bool isGamePaused { get { return GameSessionState.IsGamePaused; } set { GameSessionState.IsGamePaused = value; } }
    public static bool isFire { get { return GameSessionState.IsFire; } set { GameSessionState.IsFire = value; } }
    public static bool isUpdate { get { return GameSessionState.IsUpdate; } set { GameSessionState.IsUpdate = value; } }
    public static int buyMoreItem { get { return GameSessionState.BuyMoreItem; } set { GameSessionState.BuyMoreItem = value; } }

    public static bool won { get { return GameSessionState.Won; } set { GameSessionState.Won = value; } }
    public static bool lose { get { return GameSessionState.Lose; } set { GameSessionState.Lose = value; } }
    public static int loseCount { get { return GameSessionState.LoseCount; } set { GameSessionState.LoseCount = value; } }
    public static int levelLose { get { return GameSessionState.LevelLose; } set { GameSessionState.LevelLose = value; } }
    public static bool isLoadData { get { return GameSessionState.IsLoadData; } set { GameSessionState.IsLoadData = value; } }
    public static float isShake { get { return GameSessionState.IsShake; } set { GameSessionState.IsShake = value; } }

    public static SecuredFloat currentFireRate = new SecuredFloat(0);
    public static SecuredFloat currentSubFireRate = new SecuredFloat(0);
    public static SecuredFloat currentSpeedAtt = new SecuredFloat(0);

    public static SecuredInt lastscore = new SecuredInt(0);
    public static SecuredInt lastCoin = new SecuredInt(0);
    public static SecuredInt lastCrystal = new SecuredInt(0);
    public static SecuredInt currentCoin = new SecuredInt(0);
    public static SecuredInt currentCrystal = new SecuredInt(0);
    public static SecuredInt countStar = new SecuredInt(0);
    public static int hit { get { return GameSessionState.Hit; } set { GameSessionState.Hit = value; } }
    public static SecuredInt totalHit = new SecuredInt(0);
    public static SecuredInt totalRocketClick = new SecuredInt(3);
    public static SecuredInt totalShieldClick = new SecuredInt(3);
    public static SecuredInt totalSupportClick = new SecuredInt(3);
    public static SecuredInt dmgRocket = new SecuredInt(50);
    public static SecuredFloat speedRocket = new SecuredFloat(5);
    public static SecuredInt dmgSupport = new SecuredInt(50);
    public static SecuredFloat speedSupport = new SecuredFloat(5);
    public static SecuredInt shield = new SecuredInt(100);

    public static int boss4State { get { return GameSessionState.Boss4State; } set { GameSessionState.Boss4State = value; } }
    public static int boss3State { get { return GameSessionState.Boss3State; } set { GameSessionState.Boss3State = value; } }
    public static SecuredInt totalSaveMe = new SecuredInt();
    public static bool isReceive50 { get { return GameSessionState.IsReceive50; } set { GameSessionState.IsReceive50 = value; } }
    public static int countMiniBoss { get { return GameSessionState.CountMiniBoss; } set { GameSessionState.CountMiniBoss = value; } }
    public static int countBoss { get { return GameSessionState.CountBoss; } set { GameSessionState.CountBoss = value; } }
    public static bool isMoveMap { get { return GameSessionState.IsMoveMap; } set { GameSessionState.IsMoveMap = value; } }
    public static SecuredInt currentPlane = new SecuredInt(1);
    public static SecuredInt attackPlane = new SecuredInt(60);
    public static SecuredInt defendPlane = new SecuredInt(60);
    public static SecuredInt energyPlane = new SecuredInt(200);
    public static SecuredFloat speedAttPlane = new SecuredFloat(0);
    public static SecuredInt energyToPlay = new SecuredInt(0);
    public static SecuredInt currentLevel = new SecuredInt(1);

    public static float delayBossAtt { get { return GameSessionState.DelayBossAtt; } set { GameSessionState.DelayBossAtt = value; } }
    public static int loginDayInRow { get { return GameSessionState.LoginDayInRow; } set { GameSessionState.LoginDayInRow = value; } }
    public static int loginDayInRowOffer { get { return GameSessionState.LoginDayInRowOffer; } set { GameSessionState.LoginDayInRowOffer = value; } }
    public static int isGetBonusToday { get { return GameSessionState.IsGetBonusToday; } set { GameSessionState.IsGetBonusToday = value; } }

    public static SecuredInt countSaveMe = new SecuredInt(0);
    public static SecuredInt useRocket = new SecuredInt(0);
    public static SecuredInt useSupport = new SecuredInt(0);
    public static SecuredInt useShield = new SecuredInt(0);
    public static SecuredInt useNukeBomb = new SecuredInt(0);
    public static SecuredInt currentTotalEnemy = new SecuredInt(0);

    public static event System.Action<int, int, int> BoostCountChanged;

    public static int numberLoseLevel { get { return GameSessionState.NumberLoseLevel; } set { GameSessionState.NumberLoseLevel = value; } }
    public static bool isShowUpgrade { get { return GameSessionState.IsShowUpgrade; } set { GameSessionState.IsShowUpgrade = value; } }
    public static int planeUpgrading { get { return GameSessionState.PlaneUpgrading; } set { GameSessionState.PlaneUpgrading = value; } }
    public static string namePlaneUpgrade { get { return GameSessionState.NamePlaneUpgrade; } set { GameSessionState.NamePlaneUpgrade = value; } }
    public static double quitTime { get { return GameSessionState.QuitTime; } set { GameSessionState.QuitTime = value; } }
    public static int countLose { get { return GameSessionState.CountLose; } set { GameSessionState.CountLose = value; } }
    public static int goToMainMenuSceneTime { get { return GameSessionState.GoToMainMenuSceneTime; } set { GameSessionState.GoToMainMenuSceneTime = value; } }
    public static int countShowReskin { get { return GameSessionState.CountShowReskin; } set { GameSessionState.CountShowReskin = value; } }

    public static bool isX2Coin { get { return GameSessionState.IsX2Coin; } set { GameSessionState.IsX2Coin = value; } }
    public static int userID { get { return GameSessionState.UserID; } set { GameSessionState.UserID = value; } }

    public static int GetPlaneUpgrading()
    {
        planeUpgrading = PlayerUpgradeService.GetPlaneUpgrading();
        return planeUpgrading;
    }

    public static void SetPlaneUpgrading(int value)
    {
        PlayerUpgradeService.SetPlaneUpgrading(value);
        planeUpgrading = PlayerUpgradeService.GetPlaneUpgrading();
        if (value == 0)
            namePlaneUpgrade = string.Empty;
    }

    public static string GetNamePlaneUpgrade()
    {
        namePlaneUpgrade = PlayerUpgradeService.GetNamePlaneUpgrade();
        return namePlaneUpgrade;
    }

    public static void SetNamePlaneUpgrade(string str)
    {
        PlayerUpgradeService.SetNamePlaneUpgrade(str);
        namePlaneUpgrade = str ?? string.Empty;
    }

    public static void SaveLevelComplete(int star)
    {
        PlayerProgressService.SaveLevelComplete(currentLevel.Value, star);
    }

    public static void AddScore(int score)
    {
        lastscore.Value += score;
        if (UITextControl.Instance != null && UITextControl.Instance.scoreTxt != null)
        {
            int group1 = lastscore / 1000000;
            int group2 = (lastscore - group1 * 1000000) / 1000;
            int group3 = lastscore - group1 * 1000000 - group2 * 1000;
            UITextControl.Instance.scoreTxt.text = string.Format("{0:000} {1:000} {2:000}", group1, group2, group3);
        }
    }

    public static int GetStar()
    {
        countStar.Value = PlayerProgressService.GetStar();
        return countStar;
    }

    public static int GetAllStarLevel(int level)
    {
        countStar.Value = PlayerProgressService.GetAllStarLevel(level);
        return countStar;
    }

    public static void AddCoin(int coin)
    {
        PlayerInventoryService.AddCoin(coin, ref lastCoin);
        if (UITextControl.instance != null)
            UITextControl.instance.CheckHitReward();
    }

    public static void IncreaseHit()
    {
        hit++;
        totalHit.Value++;
        if (UITextControl.Instance != null && UITextControl.Instance.hitTxt != null)
            UITextControl.Instance.hitTxt.text = hit.ToString();
    }

    public static void AddCrystal(int crystal)
    {
        PlayerInventoryService.AddCrystal(crystal, ref lastCrystal);
    }

    public static void AddEnergy(int energy)
    {
        PlayerInventoryService.AddEnergy(energy, ref energyToPlay);
    }

    public static void SaveCoin()
    {
        PlayerInventoryService.SaveCoin(lastCoin.Value);
    }

    public static void SaveCrystal()
    {
        PlayerInventoryService.SaveCrystal(lastCrystal.Value);
    }

    public static void SaveEnergy()
    {
        PlayerInventoryService.SaveEnergy(energyToPlay.Value);
    }

    public static void SaveItem()
    {
        PlayerInventoryService.SaveItem(currentLevel.Value, totalRocketClick.Value, totalShieldClick.Value, totalSupportClick.Value);
    }

    public static void SaveHighScore()
    {
        PlayerProgressService.SaveHighScore(currentLevel.Value, lastscore.Value);
    }

    public static int GetHighScore(int index)
    {
        return PlayerProgressService.GetHighScore(index);
    }

    public static void SetHighScore(int index, int score)
    {
        PlayerProgressService.SetHighScore(index, score);
    }

    public static int GetTotalHighScore()
    {
        return PlayerProgressService.GetTotalHighScore();
    }

    public static int GetLevelStar(int index)
    {
        return PlayerProgressService.GetLevelStar(index);
    }

    public static void SetLevelStar(int index, int value)
    {
        PlayerProgressService.SetLevelStar(index, value);
    }

    public static int GetLockPlane(int index)
    {
        return PlayerUpgradeService.GetLockPlane(index);
    }

    public static void SetLockPlane(int index, int value)
    {
        PlayerUpgradeService.SetLockPlane(index, value);
    }

    public static int GetEnergy()
    {
        return PlayerInventoryService.GetEnergy();
    }

    public static void SetEnergy(int value)
    {
        PlayerInventoryService.SetEnergy(value);
    }

    public static int GetGold()
    {
        return PlayerInventoryService.GetGold();
    }

    public static void SetGold(int value)
    {
        PlayerInventoryService.SetGold(value);
    }

    public static int GetCrystal()
    {
        return PlayerInventoryService.GetCrystal();
    }

    public static void SetCrystal(int value)
    {
        PlayerInventoryService.SetCrystal(value);
    }

    public static int GetRocket()
    {
        return PlayerInventoryService.GetRocket();
    }

    public static void SetRocket(int value)
    {
        totalRocketClick.Value = Mathf.Max(0, value);
        PlayerInventoryService.SetRocket(totalRocketClick.Value);
        NotifyBoostCountChanged();
    }

    public static int GetSupport()
    {
        return PlayerInventoryService.GetSupport();
    }

    public static void SetSupport(int value)
    {
        totalSupportClick.Value = Mathf.Max(0, value);
        PlayerInventoryService.SetSupport(totalSupportClick.Value);
        NotifyBoostCountChanged();
    }

    public static int GetShield()
    {
        return PlayerInventoryService.GetShield();
    }

    public static void SetShield(int value)
    {
        totalShieldClick.Value = Mathf.Max(0, value);
        PlayerInventoryService.SetShield(totalShieldClick.Value);
        NotifyBoostCountChanged();
    }

    private static void NotifyBoostCountChanged()
    {
        System.Action<int, int, int> callback = BoostCountChanged;
        if (callback != null)
            callback(totalRocketClick.Value, totalSupportClick.Value, totalShieldClick.Value);
    }

    public static int GetLevelCompleted()
    {
        return PlayerProgressService.GetLevelCompleted();
    }

    public static void SetLevelCompleted(int value)
    {
        PlayerProgressService.SetLevelCompleted(value);
    }

    public static int GetLevelPlane(int index)
    {
        return PlayerUpgradeService.GetLevelPlane(index);
    }

    public static void SetLevelPlane(int index, int value)
    {
        PlayerUpgradeService.SetLevelPlane(index, value);
    }

    public static int GetLevelRankPlane(int index)
    {
        return PlayerUpgradeService.GetLevelRankPlane(index);
    }

    public static void SetLevelRankPlane(int index, int value)
    {
        PlayerUpgradeService.SetLevelRankPlane(index, value);
    }

    public static int GetLevelMissile()
    {
        return PlayerUpgradeService.GetLevelMissile();
    }

    public static void SetLevelMissile(int level)
    {
        PlayerUpgradeService.SetLevelMissile(level);
        levelMissile.Value = PlayerUpgradeService.GetLevelMissile();
    }

    public static int GetLevelSupport()
    {
        return PlayerUpgradeService.GetLevelSupport();
    }

    public static void SetLevelSupport(int level)
    {
        PlayerUpgradeService.SetLevelSupport(level);
        levelSupport.Value = PlayerUpgradeService.GetLevelSupport();
    }

    public static int GetLevelShield()
    {
        return PlayerUpgradeService.GetLevelShield();
    }

    public static void SetLevelShield(int level)
    {
        PlayerUpgradeService.SetLevelShield(level);
        levelShield.Value = PlayerUpgradeService.GetLevelShield();
    }

    public static void ResetAll()
    {
        GameSessionState.ResetRunState();
    }
}
