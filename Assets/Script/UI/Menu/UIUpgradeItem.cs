using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UIUpgradeItem : MonoBehaviour
{
    private const int MaxBoostLevel = 5;

    public static UIUpgradeItem instance;
    public Button btnUpgrade;
    public Button btnCancel;
    //Group1
    public TMP_Text txtInfo1;
    public TMP_Text txtInfo2;
    public TMP_Text txtInfo3;
    public TMP_Text txtIndex1;
    public TMP_Text txtIndex2;
    public TMP_Text txtIndex3;
    //Group2 
    public TMP_Text txtInfo_Upgrade1;
    public TMP_Text txtInfo_Upgrade2;
    public TMP_Text txtInfo_Upgrade3;
    public TMP_Text txtIndex_Upgrade1;
    public TMP_Text txtIndex_Upgrade2;
    public TMP_Text txtIndex_Upgrade3;
    //
    public TMP_Text txtTitle1;
    public TMP_Text txtTitle2;
    public TMP_Text txtCost;
    public Image iconUpgrade;
    public Image iconUpgrade2;
    public Sprite iconMissile;
    public Sprite iconSupport;
    public Sprite iconShield;
    public GameObject coin;
    public GameObject crystal;

    private int cost;

    private bool isClick;
    private WaitForSecondsRealtime waitUpgrade2;
    void Awake()
    {
        instance = this;
        waitUpgrade2 = new WaitForSecondsRealtime(0.8f);
    }

    void OnEnable()
    {
        instance = this;
        AddClick(btnUpgrade, Upgrade);
        AddClick(btnCancel, Cancel);
        isClick = false;
        CheckInfo();
    }
    void OnDisable()
    {
        RemoveClick(btnUpgrade, Upgrade);
        RemoveClick(btnCancel, Cancel);

        if (PanelInfo.instance != null)
        {
            PanelInfo.instance.CheckIndex();
        }
        if (coin != null)
            coin.SetActive(false);
        if (crystal != null)
            crystal.SetActive(false);
        isClick = false;
    }
    void CheckInfo()
    {
        PanelInfo panelInfo = GetPanelInfo();
        if (panelInfo == null)
            return;

        SetActiveIfPresent(coin, false);
        SetActiveIfPresent(crystal, true);

        if (panelInfo.typeUpgrade == TypeUpgradeItem.typeMissile)
        {
            if (GameState.levelMissile < MaxBoostLevel)
            {
                MissileSOData current = GameConfigService.Instance.GetMissileUpgrade(GameState.levelMissile);
                MissileSOData next = GameConfigService.Instance.GetMissileUpgrade(GameState.levelMissile + 1);
                txtInfo1.text = txtInfo_Upgrade1.text = "ATK:";
                txtInfo2.text = txtInfo_Upgrade2.text = "SPEED:";
                txtInfo3.text = txtInfo_Upgrade3.text = "NUMBER:";
                txtIndex1.text = current.attack.ToString();
                txtIndex2.text = current.speed.ToString();
                txtIndex3.text = current.number.ToString();
                txtIndex_Upgrade1.text = next.attack.ToString();
                txtIndex_Upgrade2.text = next.speed.ToString();
                txtIndex_Upgrade3.text = next.number.ToString();
                txtTitle1.text = "MISSILE - LEVEL " + GameState.levelMissile.ToString();
                txtTitle2.text = "MISSILE - LEVEL " + (GameState.levelMissile + 1).ToString();
                iconUpgrade.sprite = iconUpgrade2.sprite = iconMissile;
                cost = next.cost;
            }
            else
            {
                StartCoroutine(HidePanel());
            }
        }
        else if (panelInfo.typeUpgrade == TypeUpgradeItem.typeSupport)
        {
            if (GameState.levelSupport < MaxBoostLevel)
            {
                SupportSOData current = GameConfigService.Instance.GetSupportUpgrade(GameState.levelSupport);
                SupportSOData next = GameConfigService.Instance.GetSupportUpgrade(GameState.levelSupport + 1);
                txtInfo1.text = txtInfo_Upgrade1.text = "ATK:";
                txtInfo2.text = txtInfo_Upgrade2.text = "SPEED:";
                txtInfo3.text = txtInfo_Upgrade3.text = "TIME:";
                txtIndex1.text = current.attack.ToString();
                txtIndex2.text = current.speed.ToString();
                txtIndex3.text = current.duration.ToString();
                txtIndex_Upgrade1.text = next.attack.ToString();
                txtIndex_Upgrade2.text = next.speed.ToString();
                txtIndex_Upgrade3.text = next.duration.ToString();
                txtTitle1.text = "REINFORCE - LEVEL " + GameState.levelSupport.ToString();
                txtTitle2.text = "REINFORCE - LEVEL " + (GameState.levelSupport + 1).ToString();
                iconUpgrade.sprite = iconUpgrade2.sprite = iconSupport;
                cost = next.cost;
            }
            else
            {
                StartCoroutine(HidePanel());
            }
        }
        else if (panelInfo.typeUpgrade == TypeUpgradeItem.typeShield)
        {
            if (GameState.levelShield < MaxBoostLevel)
            {
                ShieldSOData current = GameConfigService.Instance.GetShieldUpgrade(GameState.levelShield);
                ShieldSOData next = GameConfigService.Instance.GetShieldUpgrade(GameState.levelShield + 1);
                txtInfo1.text = txtInfo_Upgrade1.text = "DEF:";
                txtInfo2.text = txtInfo_Upgrade2.text = txtIndex2.text = txtIndex_Upgrade2.text = "";
                txtInfo3.text = txtInfo_Upgrade3.text = "TIME:";
                txtIndex1.text = current.defend.ToString();
                txtIndex3.text = current.duration.ToString();
                txtIndex_Upgrade1.text = next.defend.ToString();
                txtIndex_Upgrade3.text = next.duration.ToString();
                txtTitle1.text = "SHIELD - LEVEL " + GameState.levelShield.ToString();
                txtTitle2.text = "SHIELD - LEVEL " + (GameState.levelShield + 1).ToString();
                iconUpgrade.sprite = iconUpgrade2.sprite = iconShield;
                cost = next.cost;
            }
            else
            {
                StartCoroutine(HidePanel());
            }
        }
        SetText(txtCost, cost.ToString());
    }
    void Upgrade()
    {
        PanelInfo panelInfo = GetPanelInfo();
        if (isClick || panelInfo == null)
            return;

        if (GetCurrentLevel(panelInfo.typeUpgrade) >= MaxBoostLevel)
            return;

        int currentCrystal = GameState.GetCrystal();
        if (currentCrystal < cost)
        {
            ShowNotEnoughCrystal();
            return;
        }

        GameState.SetCrystal(currentCrystal - cost);
        GameState.lastCrystal.Value = GameState.GetCrystal();
        GameState.SaveCrystal();
        if (MenuEventListener.instance != null)
            MenuEventListener.instance.UpdateText();

        isClick = true;
        ApplyUpgrade(panelInfo.typeUpgrade);
        isClick = false;
    }

    private void ApplyUpgrade(TypeUpgradeItem upgradeType)
    {
        if (upgradeType == TypeUpgradeItem.typeMissile)
        {
            int newLevel = Mathf.Min(GameState.levelMissile.Value + 1, MaxBoostLevel);
            GameState.SetLevelMissile(newLevel);
            ShowUpgradeSuccess("UPGRADE MISSILE - LEVEL " + GameState.levelMissile.ToString() + " SUCCESSFUL !");
            CheckInfo();
        }
        else if (upgradeType == TypeUpgradeItem.typeSupport)
        {
            int newLevel = Mathf.Min(GameState.levelSupport.Value + 1, MaxBoostLevel);
            GameState.SetLevelSupport(newLevel);
            ShowUpgradeSuccess("UPGRADE REINFORCE - LEVEL " + GameState.levelSupport.ToString() + " SUCCESSFUL !");
            CheckInfo();
        }
        else if (upgradeType == TypeUpgradeItem.typeShield)
        {
            int newLevel = Mathf.Min(GameState.levelShield.Value + 1, MaxBoostLevel);
            GameState.SetLevelShield(newLevel);
            ShowUpgradeSuccess("UPGRADE SHIELD - LEVEL " + GameState.levelShield.ToString() + " SUCCESSFUL !");
            CheckInfo();
        }
    }
    void Cancel()
    {
        StopAllCoroutines();
        isClick = false;
        gameObject.SetActive(false);
    }
    IEnumerator HidePanel()
    {
        yield return waitUpgrade2;
        gameObject.SetActive(false);
    }

    private static void AddClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null)
            return;

        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    private static void RemoveClick(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.RemoveListener(action);
    }

    private int GetCurrentLevel(TypeUpgradeItem type)
    {
        if (type == TypeUpgradeItem.typeMissile)
            return GameState.levelMissile;
        if (type == TypeUpgradeItem.typeSupport)
            return GameState.levelSupport;
        if (type == TypeUpgradeItem.typeShield)
            return GameState.levelShield;

        return 1;
    }

    private static PanelInfo GetPanelInfo()
    {
        if (PanelInfo.instance != null)
            return PanelInfo.instance;

        return FindObjectOfType<PanelInfo>(true);
    }

    private void ShowUpgradeSuccess(string message)
    {
        if (UIDialog.instance != null)
            UIDialog.instance.ShowDialog(message);
        else
            GameNotice.Show(message);
    }

    private void ShowNotEnoughCrystal()
    {
        if (PopupManager.instance != null)
            PopupManager.instance.ShowPopup("MESSAGE", "You not enough crystals to upgrade item!\nBUY MORE?", TYPE_POPUP.NOT_ENOUGH_CRYSTAL);
        else
            GameNotice.Show("You not enough crystals to upgrade item!");
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
}
