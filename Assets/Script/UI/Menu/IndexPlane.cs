using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IndexPlane : MonoBehaviour
{
    public static IndexPlane instance;

    public TMP_Text planeName;
    public Image[] indexCurrent;
    public TMP_Text[] indexText;
    public ColorImage colorLevel1;
    public ColorImage colorLevel2;

    private int level;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        SetInfoIndex();
    }

    public void SetInfoIndex()
    {
        PlayerPlaneConfigSO playerPlane = GameConfigService.Instance.GetPlayerPlane();
        int maxLevel = GameConfigService.Instance.GetPlayerPlaneUpgradeCount();
        if (playerPlane == null || maxLevel <= 0)
            return;

        PlayerPlaneSpriteApplicator.ApplyToMenuPreviews(playerPlane);
        level = Mathf.Clamp(GameState.GetLevelPlane(0), 1, maxLevel);
        SetText(planeName, playerPlane.namePlane + " Lv." + level.ToString());
        SetDisplayEnergy(playerPlane);
    }

    private void SetDisplayEnergy(PlayerPlaneConfigSO plane)
    {
        if (plane.upgrade == null || level - 1 < 0 || level - 1 >= plane.upgrade.Count)
            return;

        UpgradeSOData upgrade = plane.upgrade[level - 1];
        SetText(indexText, 0, upgrade.attack.ToString());
        SetText(indexText, 1, upgrade.defend.ToString());
        SetText(indexText, 2, upgrade.energy.ToString());

        SetDisplayUpgradeEnergy(upgrade.attack, GetImage(indexCurrent, 0));
        SetDisplayUpgradeEnergy(upgrade.defend, GetImage(indexCurrent, 1));
        SetDisplayUpgradeEnergy(upgrade.energy, GetImage(indexCurrent, 2));

        GameState.speedAttPlane.Value = upgrade.speed;
    }

    private void SetDisplayUpgradeEnergy(int maxEnergy, Image image)
    {
        if (image == null)
            return;

        bool isChangeLevel = maxEnergy >= 500;
        float ratio = isChangeLevel ? colorLevel2.ratio : colorLevel1.ratio;
        Color colorCurrent = isChangeLevel ? colorLevel2.colorImg : colorLevel1.colorImg;

        image.fillAmount = ratio * maxEnergy;
        image.color = colorCurrent;
    }

    private static Image GetImage(Image[] images, int index)
    {
        if (images == null || index < 0 || index >= images.Length)
            return null;

        return images[index];
    }

    private static void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value;
    }

    private static void SetText(TMP_Text[] texts, int index, string value)
    {
        if (texts != null && index >= 0 && index < texts.Length && texts[index] != null)
            texts[index].text = value;
    }
}
