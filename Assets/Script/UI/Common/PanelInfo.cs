using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelInfo : MonoBehaviour
{
    public static PanelInfo instance;

    public TMP_Text totalShield;
    public TMP_Text totalSupport;
    public TMP_Text totalRocket;
    public TMP_Text txtLevelMissile;
    public TMP_Text txtLevelSupport;
    public TMP_Text txtLevelShield;
    public Button close;
    public Button upgradeBoostButton;
    public Button upgradePlaneButton;
    public GameObject planeUpgradeDialog;
    public GameObject iconUpgradeMissile;
    public GameObject iconUpgradeSupport;
    public GameObject iconUpgradeShield;

    public TMP_Text txtMissileIndex1;
    public TMP_Text txtMissileIndex2;
    public TMP_Text txtMissileIndex3;
    public TMP_Text txtSupportIndex1;
    public TMP_Text txtSupportIndex2;
    public TMP_Text txtSupportIndex3;
    public TMP_Text txtShieldIndex1;
    public TMP_Text txtShieldIndex2;

    public GameObject maxMissile;
    public GameObject maxSupport;
    public GameObject maxShield;
    public TypeUpgradeItem typeUpgrade;

    public GameObject panelUpgrade;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void ShowUpgrade(int type)
    {
        if (type == (int)TypeUpgradeItem.typeMissile)
            TryShowItemUpgrade(TypeUpgradeItem.typeMissile, GameState.levelMissile, "MISSILE ALREADY MAXIMUM LEVEL");
        else if (type == (int)TypeUpgradeItem.typeSupport)
            TryShowItemUpgrade(TypeUpgradeItem.typeSupport, GameState.levelSupport, "REINFORCE ALREADY MAXIMUM LEVEL");
        else if (type == (int)TypeUpgradeItem.typeShield)
            TryShowItemUpgrade(TypeUpgradeItem.typeShield, GameState.levelShield, "SHIELD ALREADY MAXIMUM LEVEL");
    }

    public void OpenBoostInfoPanel()
    {
        SetActiveIfPresent(gameObject, true);
        CheckIndex();
    }

    public void OpenPlaneUpgradeDialog()
    {
        SetActiveIfPresent(planeUpgradeDialog, true);
    }

    private void TryShowItemUpgrade(TypeUpgradeItem upgradeType, int currentLevel, string maxMessage)
    {
        typeUpgrade = upgradeType;
        if (currentLevel < 5)
        {
            SetActiveIfPresent(panelUpgrade, true);
            return;
        }

        if (UIDialog.instance != null)
            UIDialog.instance.ShowDialog(maxMessage);
    }

    private void OnEnable()
    {
        instance = this;
        AddClick(close, ClosePopup);
        AddClick(upgradeBoostButton, OpenBoostInfoPanel);
        AddClick(upgradePlaneButton, OpenPlaneUpgradeDialog);

        RefreshItemCountTexts();
        CheckIndex();
    }

    private void OnDisable()
    {
        RemoveClick(close, ClosePopup);
        RemoveClick(upgradeBoostButton, OpenBoostInfoPanel);
        RemoveClick(upgradePlaneButton, OpenPlaneUpgradeDialog);
    }

    public void CheckIndex()
    {
        int levelMissile = Mathf.Min(GameState.levelMissile, 5);
        int levelSupport = Mathf.Min(GameState.levelSupport, 5);
        int levelShield = Mathf.Min(GameState.levelShield, 5);
        MissileSOData missile = GameConfigService.Instance.GetMissileUpgrade(levelMissile);
        SupportSOData support = GameConfigService.Instance.GetSupportUpgrade(levelSupport);
        ShieldSOData shield = GameConfigService.Instance.GetShieldUpgrade(levelShield);

        if (missile != null)
        {
            SetText(txtLevelMissile, missile.upgrade);
            SetText(txtMissileIndex1, missile.attack.ToString());
            SetText(txtMissileIndex2, missile.number.ToString());
            SetText(txtMissileIndex3, missile.speed.ToString());
        }

        if (support != null)
        {
            SetText(txtLevelSupport, support.upgrade);
            SetText(txtSupportIndex1, support.attack.ToString());
            SetText(txtSupportIndex2, support.speed.ToString());
            SetText(txtSupportIndex3, support.duration.ToString());
        }

        if (shield != null)
        {
            SetText(txtLevelShield, shield.upgrade);
            SetText(txtShieldIndex1, shield.defend.ToString());
            SetText(txtShieldIndex2, shield.duration.ToString());
        }

        SetMaxState(maxMissile, iconUpgradeMissile, GameState.levelMissile >= 5);
        SetMaxState(maxShield, iconUpgradeShield, GameState.levelShield >= 5);
        SetMaxState(maxSupport, iconUpgradeSupport, GameState.levelSupport >= 5);
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }

    private void RefreshItemCountTexts()
    {
        SetText(totalShield, GameState.GetShield().ToString());
        SetText(totalRocket, GameState.GetRocket().ToString());
        SetText(totalSupport, GameState.GetSupport().ToString());
    }

    private static void AddClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
        {
            button.onClick.RemoveListener(action);
            button.onClick.AddListener(action);
        }
    }

    private static void RemoveClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.RemoveListener(action);
    }

    private static void SetActiveIfPresent(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }

    private static void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value;
    }

    private static void SetMaxState(GameObject maxLabel, GameObject upgradeIcon, bool isMax)
    {
        SetActiveIfPresent(maxLabel, isMax);
        SetActiveIfPresent(upgradeIcon, !isMax);
    }
}

public enum TypeUpgradeItem
{
    typeMissile = 1,
    typeSupport = 2,
    typeShield = 3
}
