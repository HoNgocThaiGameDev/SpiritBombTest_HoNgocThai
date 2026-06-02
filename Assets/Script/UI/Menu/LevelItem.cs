using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public GameObject textLevel;
    public GameObject[] star;
    public GameObject starGroup;
    public int level;
    public GameObject lockItem;
    public GameObject effectSelect;
    public Image imgItem;

    private void Start()
    {
        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        int earnedStars = Mathf.Clamp(GameState.GetLevelStar(level - 1), 0, star != null ? star.Length : 0);
        if (star != null)
        {
            for (int i = 0; i < star.Length; i++)
            {
                if (star[i] != null)
                    star[i].SetActive(i < earnedStars);
            }
        }

        bool unlocked = lockItem == null || !lockItem.activeInHierarchy;
        if (starGroup != null)
            starGroup.SetActive(unlocked);
        if (textLevel != null)
            textLevel.SetActive(unlocked);

        if (unlocked)
        {
            SetBaseStarObjectsActive(false);
        }

        if (level == GameState.GetLevelCompleted() + 1)
        {
            if (effectSelect != null)
                effectSelect.SetActive(true);
            if (imgItem != null)
                imgItem.color = Color.white;
        }
    }

    private void SetBaseStarObjectsActive(bool active)
    {
        if (starGroup == null)
            return;

        for (int i = 0; i < starGroup.transform.childCount; i++)
        {
            Transform child = starGroup.transform.GetChild(i);
            if (child.name == "StarLight")
                continue;

            child.gameObject.SetActive(active);
        }
    }
}
