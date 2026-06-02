public static class GameSessionState
{
    public static bool LevelFull = false;
    public static bool PlaneFull = true;
    public static bool IsShowAnim = true;
    public static bool IsX2Reward = false;
    public static bool IsSaveMe = false;
    public static bool IsReduce10Percent = false;
    public static bool IsRestartFire = false;
    public static string Language = "ThaiLan";
    public static int CurrentLevelPlane = 0;
    public static int CountWin = 0;

    public static SecuredInt LevelMissile = new SecuredInt(1);
    public static SecuredInt LevelSupport = new SecuredInt(1);
    public static SecuredInt LevelShield = new SecuredInt(1);

    public static string[] ListTip;
    public static string[] ListSlogan;
    public static string[] ListMapName;

    public static bool IsGamePaused;
    public static bool IsFire;
    public static bool IsUpdate = true;
    public static int BuyMoreItem = 0;

    public static bool Won = false;
    public static bool Lose = false;
    public static int LoseCount = 0;
    public static int LevelLose = 0;
    public static bool IsLoadData = false;
    public static float IsShake;

    public static SecuredFloat CurrentFireRate = new SecuredFloat(0);
    public static SecuredFloat CurrentSubFireRate = new SecuredFloat(0);
    public static SecuredFloat CurrentSpeedAtt = new SecuredFloat(0);

    public static SecuredInt LastScore = new SecuredInt(0);
    public static SecuredInt LastCoin = new SecuredInt(0);
    public static SecuredInt LastCrystal = new SecuredInt(0);
    public static SecuredInt CurrentCoin = new SecuredInt(0);
    public static SecuredInt CurrentCrystal = new SecuredInt(0);
    public static SecuredInt CountStar = new SecuredInt(0);
    public static int Hit;
    public static SecuredInt TotalHit = new SecuredInt(0);

    public static SecuredInt TotalRocketClick = new SecuredInt(3);
    public static SecuredInt TotalShieldClick = new SecuredInt(3);
    public static SecuredInt TotalSupportClick = new SecuredInt(3);
    public static SecuredInt DmgRocket = new SecuredInt(50);
    public static SecuredFloat SpeedRocket = new SecuredFloat(5);
    public static SecuredInt DmgSupport = new SecuredInt(50);
    public static SecuredFloat SpeedSupport = new SecuredFloat(5);
    public static SecuredInt Shield = new SecuredInt(100);

    public static int Boss4State = 1;
    public static int Boss3State = 1;
    public static SecuredInt TotalSaveMe = new SecuredInt();
    public static bool IsReceive50 = false;
    public static int CountMiniBoss = 0;
    public static int CountBoss = 0;
    public static bool IsMoveMap = true;
    public static SecuredInt CurrentPlane = new SecuredInt(1);
    public static SecuredInt AttackPlane = new SecuredInt(60);
    public static SecuredInt DefendPlane = new SecuredInt(60);
    public static SecuredInt EnergyPlane = new SecuredInt(200);
    public static SecuredFloat SpeedAttPlane = new SecuredFloat(0);
    public static SecuredInt EnergyToPlay = new SecuredInt(0);
    public static SecuredInt CurrentLevel = new SecuredInt(1);

    public static float DelayBossAtt = 4;
    public static int LoginDayInRow = 1;
    public static int LoginDayInRowOffer = 1;
    public static int IsGetBonusToday;

    public static SecuredInt CountSaveMe = new SecuredInt(0);
    public static SecuredInt UseRocket = new SecuredInt(0);
    public static SecuredInt UseSupport = new SecuredInt(0);
    public static SecuredInt UseShield = new SecuredInt(0);
    public static SecuredInt UseNukeBomb = new SecuredInt(0);
    public static SecuredInt CurrentTotalEnemy = new SecuredInt(0);

    public static int NumberLoseLevel = 0;
    public static bool IsShowUpgrade = false;
    public static int PlaneUpgrading = 0;
    public static string NamePlaneUpgrade = "";
    public static double QuitTime = 0;
    public static int CountLose = 0;
    public static int GoToMainMenuSceneTime = 0;
    public static int CountShowReskin = 0;

    public static bool IsX2Coin = false;
    public static int UserID;

    public static void ResetRunState()
    {
        IsX2Reward = false;
        UseNukeBomb.Value = 0;
        UseRocket.Value = 0;
        UseSupport.Value = 0;
        UseShield.Value = 0;
        CountBoss = 0;
        CountMiniBoss = 0;
        CountSaveMe.Value = 0;
        Lose = false;
        Won = false;
        LastScore.Value = 0;
        Hit = 0;
        TotalHit.Value = 0;
        TotalSaveMe.Value = 0;
        CurrentCoin.Value = 0;
        CurrentCrystal.Value = 0;
    }
}
