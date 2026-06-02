using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeBannerControl : MonoBehaviour
{
    public Button yes;
    public Button no;
    public GameObject showItemPanel;
    private void OnEnable()
    {
        AddClick(yes, YesClick);
        AddClick(no, NoClick);
    }

    private void OnDisable()
    {
        RemoveClick(yes, YesClick);
        RemoveClick(no, NoClick);
    }

    private void YesClick()
    {
        gameObject.SetActive(false);
        if (showItemPanel != null)
            showItemPanel.SetActive(true);
    }

    private void NoClick()
    {
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
}
