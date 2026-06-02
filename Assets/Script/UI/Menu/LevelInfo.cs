using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    public TMP_Text txtLevel;
    public TMP_Text txtNameLevel;
    public TMP_Text txtEnergyUse;
    public GameObject[] starList;
    public TMP_Text txtSlogan;
    private void OnEnable()
    {
        ResetDisplay();

        if (txtLevel != null)
            txtLevel.text = "LEVEL " + GameState.currentLevel;
        if (txtEnergyUse != null)
            txtEnergyUse.text = GetCurrentLevelEnergyCost().ToString();
        if (txtSlogan != null && GameState.listSlogan != null && GameState.listSlogan.Length > 0)
        {
            txtSlogan.gameObject.SetActive(true);
            txtSlogan.text = GameState.listSlogan[Random.Range(0, GameState.listSlogan.Length)];
        }

        GetNameLevel();
        CheckStar();
    }

    private int GetCurrentLevelEnergyCost()
    {
        try
        {
            return GameConfigService.Instance.GetLevelEnergyCost(GameState.currentLevel.Value);
        }
        catch (System.Exception exception)
        {
            Debug.LogWarning("[LevelInfo] Could not read energy cost for level " + GameState.currentLevel.Value + ": " + exception.Message);
            return 0;
        }
    }

    private void CheckStar()
    {
        int starCount = Mathf.Min(GameState.GetLevelStar(GameState.currentLevel - 1), starList != null ? starList.Length : 0);
        for (int i = 0; i < starCount; i++)
        {
            if (starList[i] != null)
                starList[i].SetActive(true);
        }
    }

    private void GetNameLevel()
    {
        if (txtNameLevel == null)
            return;

        if (GameState.currentLevel >= 1 && GameState.currentLevel <= 8)
            txtNameLevel.text = StringManager.MapName1;
        else if (GameState.currentLevel >= 9 && GameState.currentLevel <= 16)
            txtNameLevel.text = StringManager.MapName2;
        else if (GameState.currentLevel >= 17 && GameState.currentLevel <= 23)
            txtNameLevel.text = StringManager.MapName3;
        else if (GameState.currentLevel >= 24 && GameState.currentLevel <= 30)
            txtNameLevel.text = StringManager.MapName4;
        else if (GameState.currentLevel >= 31 && GameState.currentLevel <= 38)
            txtNameLevel.text = StringManager.MapName5;
        else if (GameState.currentLevel >= 39 && GameState.currentLevel <= 46)
            txtNameLevel.text = StringManager.MapName6;
        else if (GameState.currentLevel >= 47 && GameState.currentLevel <= 53)
            txtNameLevel.text = StringManager.MapName7;
        else if (GameState.currentLevel >= 54 && GameState.currentLevel <= 60)
            txtNameLevel.text = StringManager.MapName8;
    }

    private void OnDisable()
    {
        ResetDisplay();
    }

    private void ResetDisplay()
    {
        for (int i = 0; starList != null && i < starList.Length; i++)
        {
            if (starList[i] != null)
                starList[i].SetActive(false);
        }
        if (txtSlogan != null)
            txtSlogan.gameObject.SetActive(false);
    }
}
