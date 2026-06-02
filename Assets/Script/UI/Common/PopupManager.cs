using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{

    private static PopupManager _instance;
    public static PopupManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PopupManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("PopupManager_Fallback");
                    _instance = go.AddComponent<PopupManager>();
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
    public TMP_Text txtTitle;
    public TMP_Text txtContent;
    public GameObject popup;
    TYPE_POPUP typePopup;
    private WaitForSeconds timeWait;
    public bool isHideUpgradeItem = false;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        typePopup = new TYPE_POPUP();
        timeWait = new WaitForSeconds(1);
    }

    public void ShowPopup(string title, string content, TYPE_POPUP type)
    {
        typePopup = type;
        txtTitle.text = title;
        txtContent.text = content;
        popup.SetActive(true);
    }

    public void ClickYes()
    {
        if (MenuEventListener.instance != null)
        {
            isHideUpgradeItem = true;
            if (!MenuEventListener.instance.LastUpgradeCanvas.activeInHierarchy)
            {
                MenuEventListener.instance.canvasInfo.SetActive(false);
            }
            MenuEventListener.instance.ItemClick();
        }
        popup.SetActive(false);
    }

    IEnumerator WaitShowShop()
    {
        yield return timeWait;
        if (MenuEventListener.instance != null)
        {
            MenuEventListener.instance.ItemClick();
        }
    }

    public void ClickNo()
    {
        popup.SetActive(false);
    }

    public void InitializeFallbackReferences()
    {
        GameObject root = new GameObject("DummyPopupContainer");
        root.transform.SetParent(this.transform);
        root.SetActive(false);
        
        popup = new GameObject("popup");
        popup.transform.SetParent(root.transform);
        popup.SetActive(false);
        
        GameObject titleGo = new GameObject("txtTitle");
        titleGo.transform.SetParent(root.transform);
        txtTitle = titleGo.AddComponent<TextMeshProUGUI>();
        
        GameObject contentGo = new GameObject("txtContent");
        contentGo.transform.SetParent(root.transform);
        txtContent = contentGo.AddComponent<TextMeshProUGUI>();
    }
}


