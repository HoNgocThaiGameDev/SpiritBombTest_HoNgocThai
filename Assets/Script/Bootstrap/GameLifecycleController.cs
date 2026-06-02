using System;
using UnityEngine;

public class GameLifecycleController : Singleton<GameLifecycleController>
{
    private double lastInterval;

    protected override void Awake()
    {
        base.Awake();
        Application.runInBackground = true;
    }

    private void Start()
    {
        CheckRegenEnergy();
        lastInterval = Time.realtimeSinceStartup;
    }

    public void CheckRegenEnergy()
    {
        GameState.energyToPlay.Value = GameState.GetEnergy();
        if (GameState.energyToPlay.Value >= GameState.MAX_ENERGY_REGEN)
            return;

        DateTime currentTime = DateTimeUtil.GetCurrentTime();
        DateTime lastQuitTime = GetLastQuitTime();
        int regenEnergy = (int)((currentTime - lastQuitTime).TotalSeconds / GameState.ENERGY_REGEN_INTERVAL);

        if (regenEnergy <= 0)
            return;

        GameState.energyToPlay.Value = Mathf.Min(GameState.MAX_ENERGY_REGEN, GameState.energyToPlay.Value + regenEnergy);
        GameState.SetEnergy(GameState.energyToPlay.Value);
    }

    private void Update()
    {
        if (GameState.energyToPlay >= GameState.MAX_ENERGY_REGEN)
            return;

        float timeNow = Time.realtimeSinceStartup;
        int remainingSeconds = GameState.ENERGY_REGEN_INTERVAL - (int)(timeNow - lastInterval);
        UpdateEnergyCountdownDisplays(remainingSeconds);

        if (timeNow <= lastInterval + GameState.ENERGY_REGEN_INTERVAL)
            return;

        GameState.energyToPlay.Value++;
        lastInterval = timeNow;
        GameState.SaveEnergy();
        RefreshEnergyDisplays();
    }

    private void UpdateEnergyCountdownDisplays(int time)
    {
        if (MenuEventListener.instance != null)
            MenuEventListener.instance.UpdateTxtCounter(time);

        if (SelectLevelControl.instance != null)
            SelectLevelControl.instance.UpdateTextEnergy(time);
    }

    private void RefreshEnergyDisplays()
    {
        if (MenuEventListener.instance != null)
            MenuEventListener.instance.UpdateText();

        if (SelectLevelControl.instance != null)
            SelectLevelControl.instance.UpdateText();
    }

    private void OnApplicationQuit()
    {
        SaveLifecycleState();
    }

    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
            SaveLifecycleState();
    }

    private void SaveLifecycleState()
    {
        SaveTimeUpgradePlane();
        PlayerSettingsService.SetQuitTime(DateTimeUtil.GetNistTimeInMilliseconds());
    }

    private void SaveTimeUpgradePlane()
    {
        int upgradingPlane = GameState.GetPlaneUpgrading();
        if (upgradingPlane == 0)
            return;

        int remainingTime = GetUpgradeRemainingTime(upgradingPlane);
        PlayerUpgradeService.SetTimeUpgrade(remainingTime);
    }

    private int GetUpgradeRemainingTime(int upgradingPlane)
    {
        int savedTime = PlayerUpgradeService.GetTimeUpgrade();
        if (savedTime > 0)
            return Mathf.Max(0, savedTime - (int)UIConfig.timeCountUpgrade);

        int planeIndex = Mathf.Max(0, upgradingPlane - 1);
        int level = Mathf.Min(GameState.GetLevelPlane(planeIndex), UIConfig.TIME_UPGRADE.Length - 1);
        return Mathf.Max(0, UIConfig.TIME_UPGRADE[level] - (int)UIConfig.timeCountUpgrade);
    }

    private static DateTime GetLastQuitTime()
    {
        float quitTime = PlayerSettingsService.GetQuitTime();
        return DateTimeUtil.NistTimeToDatetime(quitTime);
    }
}
