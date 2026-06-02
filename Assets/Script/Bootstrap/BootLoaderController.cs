using UnityEngine;
using UnityEngine.UI;

public sealed class BootLoaderController : MonoBehaviour
{
    private Button tapToPlay;
    private Button confirmQuit;
    private Button cancelQuit;
    private GameObject quitPanel;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        tapToPlay = FindButton("TapToPlay");
        confirmQuit = FindButton("QuitPanel/Quit/Yes");
        cancelQuit = FindButton("QuitPanel/Quit/No");
        quitPanel = FindObject("QuitPanel");

        if (tapToPlay != null)
            tapToPlay.onClick.AddListener(LoadMenu);
        if (confirmQuit != null)
            confirmQuit.onClick.AddListener(QuitGame);
        if (cancelQuit != null)
            cancelQuit.onClick.AddListener(HideQuitPanel);

        InitializeCoreText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SetQuitPanelActive(quitPanel == null || !quitPanel.activeSelf);
    }

    private void OnDestroy()
    {
        if (tapToPlay != null)
            tapToPlay.onClick.RemoveListener(LoadMenu);
        if (confirmQuit != null)
            confirmQuit.onClick.RemoveListener(QuitGame);
        if (cancelQuit != null)
            cancelQuit.onClick.RemoveListener(HideQuitPanel);
    }

    public void LoadMenu()
    {
        SceneFlowService.LoadMenu(true);
    }

    public void QuitGame()
    {
        SetQuitPanelActive(false);
        Application.Quit();
    }

    public void HideQuitPanel()
    {
        SetQuitPanelActive(false);
    }

    private Button FindButton(string path)
    {
        var target = transform.Find(path);
        return target == null ? null : target.GetComponent<Button>();
    }

    private GameObject FindObject(string path)
    {
        var target = transform.Find(path);
        return target == null ? null : target.gameObject;
    }

    private void SetQuitPanelActive(bool isActive)
    {
        if (quitPanel != null)
            quitPanel.SetActive(isActive);
    }

    private static void InitializeCoreText()
    {
        GameState.listTip = new[]
        {
            StringManager.Tip1,
            StringManager.Tip2,
            StringManager.Tip3,
            StringManager.Tip4,
            StringManager.Tip5,
            StringManager.Tip6,
            StringManager.Tip7,
            StringManager.Tip8,
            StringManager.Tip9,
            StringManager.Tip10
        };

        GameState.listSlogan = new[]
        {
            StringManager.Slogan1,
            StringManager.Slogan2,
            StringManager.Slogan3,
            StringManager.Slogan4,
            StringManager.Slogan5
        };
    }
}
