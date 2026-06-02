using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IndexPlaneUpdate : MonoBehaviour
{
    public static IndexPlaneUpdate instance;
    public Image[] indexAdd;
    public Image[] indexCurrent;
    public TMP_Text[] indexText;
    public TMP_Text[] indexTextAdd;
    int att;
    int def;
    int energy;
    int addAtt;
    int addDef;
    int addEnergy;
    int level;

    public ColorImage colorLevel1;
    public ColorImage colorLevel2;
    [SerializeField] private Sprite backgroundUpgradeSprite;
    [SerializeField] private Sprite currentUpgradeSprite;
    [SerializeField] private Sprite nextUpgradeSprite;
    [SerializeField] private Color backgroundUpgradeColor = Color.black;
    [SerializeField] private Color currentUpgradeColor = Color.white;
    [SerializeField] private Color nextUpgradeColor = Color.white;

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        SetInfoIndex();
    }

    public void SetInfoIndex()
    {
        int maxLevel = GameConfigService.Instance.GetPlayerPlaneUpgradeCount();
        if (maxLevel <= 0)
            return;

        level = Mathf.Clamp(GameState.GetLevelPlane(0), 1, maxLevel);
        bool canUpgrade = level < maxLevel;

        SetActive(indexCurrent, true);
        SetActive(indexAdd, canUpgrade);
        EnsureBarRenderOrder();
        SetBarBackgrounds();

        PlayerPlaneConfigSO playerPlane = GameConfigService.Instance.GetPlayerPlane();
        if (playerPlane != null)
        {
            SetInfo(playerPlane, canUpgrade);
        }

        SetText(indexText, 0, att.ToString());
        SetText(indexText, 1, def.ToString());
        SetText(indexText, 2, energy.ToString());
        if (canUpgrade)
        {
            SetText(indexTextAdd, 0, "+" + (addAtt - att).ToString());
            SetText(indexTextAdd, 1, "+" + (addDef - def).ToString());
            SetText(indexTextAdd, 2, "+" + (addEnergy - energy).ToString());
        }
        else
        {
            SetText(indexTextAdd, 0, "");
            SetText(indexTextAdd, 1, "");
            SetText(indexTextAdd, 2, "");
        }
    }

    private void SetDisplayUpgradeEnergy(int maxEnergy, Image img, Sprite sprite, Color color)
    {
        if (img == null)
            return;

        bool isChangeLevel = maxEnergy >= 500;
        float ratio = isChangeLevel ? GetRatio(colorLevel2, 0.0002f) : GetRatio(colorLevel1, 0.002f);

        ConfigureFillImage(img, sprite, color);
        img.fillAmount = Mathf.Clamp01(ratio * maxEnergy);
        img.gameObject.SetActive(true);
    }

    private void SetInfo(PlayerPlaneConfigSO plane, bool canUpgrade)
    {
        if (plane.upgrade == null || level - 1 < 0 || level - 1 >= plane.upgrade.Count)
            return;

        att = plane.upgrade[level - 1].attack;
        def = plane.upgrade[level - 1].defend;
        energy = plane.upgrade[level - 1].energy;

        SetDisplayUpgradeEnergy(att, GetImage(indexCurrent, 0), currentUpgradeSprite, currentUpgradeColor);
        SetDisplayUpgradeEnergy(def, GetImage(indexCurrent, 1), currentUpgradeSprite, currentUpgradeColor);
        SetDisplayUpgradeEnergy(energy, GetImage(indexCurrent, 2), currentUpgradeSprite, currentUpgradeColor);

        if (canUpgrade && level < plane.upgrade.Count)
        {
            addAtt = plane.upgrade[level].attack;
            addDef = plane.upgrade[level].defend;
            addEnergy = plane.upgrade[level].energy;

            SetDisplayUpgradeEnergy(addAtt, GetImage(indexAdd, 0), nextUpgradeSprite, nextUpgradeColor);
            SetDisplayUpgradeEnergy(addDef, GetImage(indexAdd, 1), nextUpgradeSprite, nextUpgradeColor);
            SetDisplayUpgradeEnergy(addEnergy, GetImage(indexAdd, 2), nextUpgradeSprite, nextUpgradeColor);
        }
    }

    private static void SetActive(Image[] images, bool active)
    {
        if (images == null)
            return;

        for (int i = 0; i < images.Length; i++)
        {
            if (images[i] != null)
                images[i].gameObject.SetActive(active);
        }
    }

    private static Image GetImage(Image[] images, int index)
    {
        if (images == null || index < 0 || index >= images.Length)
            return null;

        return images[index];
    }

    private void EnsureBarRenderOrder()
    {
        int count = Mathf.Max(indexCurrent != null ? indexCurrent.Length : 0, indexAdd != null ? indexAdd.Length : 0);
        for (int i = 0; i < count; i++)
        {
            Image add = GetImage(indexAdd, i);
            Image current = GetImage(indexCurrent, i);

            if (add != null)
                add.transform.SetAsFirstSibling();
            if (current != null)
                current.transform.SetAsLastSibling();
        }
    }

    private void SetBarBackgrounds()
    {
        SetBarBackgrounds(indexCurrent);
        SetBarBackgrounds(indexAdd);
    }

    private void SetBarBackgrounds(Image[] images)
    {
        if (images == null)
            return;

        for (int i = 0; i < images.Length; i++)
        {
            Image image = images[i];
            if (image == null || image.transform.parent == null)
                continue;

            Image background = image.transform.parent.GetComponent<Image>();
            if (background != null)
            {
                if (backgroundUpgradeSprite != null)
                    background.sprite = backgroundUpgradeSprite;
                background.type = Image.Type.Sliced;
                background.color = backgroundUpgradeColor;
            }
        }
    }

    private static void ConfigureFillImage(Image image, Sprite sprite, Color color)
    {
        if (sprite != null)
            image.sprite = sprite;

        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Horizontal;
        image.fillOrigin = (int)Image.OriginHorizontal.Left;
        image.fillClockwise = true;
        image.color = color;
    }

    private static float GetRatio(ColorImage colorImage, float fallback)
    {
        if (colorImage == null || colorImage.ratio <= 0f)
            return fallback;

        return colorImage.ratio;
    }

    private static void SetText(TMP_Text[] texts, int index, string value)
    {
        if (texts != null && index >= 0 && index < texts.Length && texts[index] != null)
            texts[index].text = value;
    }
}
