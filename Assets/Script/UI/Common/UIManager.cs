using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("UIManager_Fallback");
                    _instance = go.AddComponent<UIManager>();
                    DontDestroyOnLoad(go);
                    _instance.InitializeFallbackReferences();
                }
            }

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    public GameObject panelLoading;
    public GameObject panelEnoughRecommend;
    public GameObject popupReward;

    public int goldEnough;
    public int energyEnough;
    public int crystalEnough;
    public bool isStartTime = true;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OpenPanelLoading()
    {
        if (panelLoading != null)
            panelLoading.SetActive(true);
    }

    public void ClosePanelLoading()
    {
        if (panelLoading != null)
            panelLoading.SetActive(false);
        if (UILoading.instance != null)
            UILoading.instance.StopCoroutine();
    }

    public void ClosePanelGetFullEnergy()
    {
        if (panelEnoughRecommend != null)
            panelEnoughRecommend.SetActive(false);
        SceneFlowService.LoadMenu(false);
    }

    public int GetBestScoreReal()
    {
        return UIConfig.highScore;
    }

    public void SetScore(int score, int idPlane, int level)
    {
    }

    public void CheckTimeUpgrade()
    {
    }

    public void CountDownUpgrade()
    {
    }

    public void SaveTimeUpgrade()
    {
    }

    public System.Collections.IEnumerator SendGiftStart()
    {
        yield break;
    }

    private void InitializeFallbackReferences()
    {
        GameObject root = new GameObject("DummyPanelsContainer");
        root.transform.SetParent(transform);
        root.SetActive(false);

        panelLoading = CreateDummyPanel(root, "panelLoading");
        panelEnoughRecommend = CreateDummyPanel(root, "panelEnoughRecommend");
        popupReward = CreateDummyPanel(root, "popupReward");
    }

    private static GameObject CreateDummyPanel(GameObject parent, string name)
    {
        GameObject dummy = new GameObject(name);
        dummy.transform.SetParent(parent.transform);
        dummy.SetActive(false);
        return dummy;
    }
}
