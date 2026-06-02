using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupNotEnoughItem : MonoBehaviour
{
    private const int MissileCost = 15;
    private const int SupportCost = 25;
    private const int ShieldCost = 15;
    private const int BuyAmount = 10;

    public TMP_Text txtContentEnoughItem;
    public TMP_Text txtCrystal;
    public TMP_Text txtCost;
    public Image imgIcon;
    public Sprite spriteShield;
    public Sprite spriteSupport;
    public Sprite spriteMissile;

    void OnEnable()
    {
        GameState.isMoveMap = false;
        SetText(txtCrystal, GameState.GetCrystal().ToString());
        RefreshPopupInfo();
        Time.timeScale = 0;
    }

    private void RefreshPopupInfo()
    {
        if (GameState.buyMoreItem == 1)
        {
            SetSprite(spriteMissile);
            SetText(txtContentEnoughItem, "YOU DON'T HAVE ENOUGH MISSILE");
            SetText(txtCost, MissileCost.ToString());
        }
        else if (GameState.buyMoreItem == 2)
        {
            SetSprite(spriteSupport);
            SetText(txtContentEnoughItem, "YOU DON'T HAVE ENOUGH REINFORCE");
            SetText(txtCost, SupportCost.ToString());
        }
        else if (GameState.buyMoreItem == 3)
        {
            SetSprite(spriteShield);
            SetText(txtContentEnoughItem, "YOU DON'T HAVE ENOUGH SHIELD");
            SetText(txtCost, ShieldCost.ToString());
        }
    }

    void OnDisable()
    {
        GameState.isMoveMap = true;
        Time.timeScale = 1;
    }

    public void ClickBuyItem()
    {
        int itemType = GameState.buyMoreItem;
        int cost = GetItemCost(itemType);
        if (cost <= 0)
            return;

        if (GameState.GetCrystal() >= cost)
        {
            GameState.AddCrystal(-cost);
            GameState.SaveCrystal();
            AddItem(itemType);
            ShowDialog(GetSuccessMessage(itemType));
        }
        else
        {
            ShowDialog("YOU DON'T HAVE ENOUGH CRYSTALS !");
        }

        gameObject.SetActive(false);
    }

    public void ClickClose()
    {
        gameObject.SetActive(false);
    }

    private void ShowDialog(string message)
    {
        if (UIDialog.instance != null)
            UIDialog.instance.ShowDialog(message);
    }

    private static int GetItemCost(int itemType)
    {
        if (itemType == 1)
            return MissileCost;
        if (itemType == 2)
            return SupportCost;
        if (itemType == 3)
            return ShieldCost;

        return 0;
    }

    private static void AddItem(int itemType)
    {
        if (itemType == 1)
            GameState.SetRocket(GameState.totalRocketClick.Value + BuyAmount);
        else if (itemType == 2)
            GameState.SetSupport(GameState.totalSupportClick.Value + BuyAmount);
        else if (itemType == 3)
            GameState.SetShield(GameState.totalShieldClick.Value + BuyAmount);
    }

    private static string GetSuccessMessage(int itemType)
    {
        if (itemType == 1)
            return "RECEIVE MISSILE x10 SUCCESSFUL !";
        if (itemType == 2)
            return "RECEIVE REINFORCE x10 SUCCESSFUL !";
        if (itemType == 3)
            return "RECEIVE SHIELD x10 SUCCESSFUL !";

        return string.Empty;
    }

    private void SetSprite(Sprite sprite)
    {
        if (imgIcon != null)
            imgIcon.sprite = sprite;
    }

    private static void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value;
    }
}
