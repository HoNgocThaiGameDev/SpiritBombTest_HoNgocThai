using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneFlowService
{
    public static void LoadMenu(bool showLoading)
    {
        Load(StringManager.SCENE_MENU, showLoading);
    }

    public static void LoadSelectLevel()
    {
        Load(StringManager.SCENE_SELECTLEVEL, false);
    }

    public static void LoadGameplay(bool showLoading)
    {
        Load(StringManager.SCENE_GAMEPLAY, showLoading);
    }

    public static void RetryGameplay()
    {
        LoadGameplay(false);
    }

    private static void Load(string destinationScene, bool showLoading)
    {
        if (!Application.CanStreamedLevelBeLoaded(destinationScene))
        {
            Debug.LogError("[SceneFlowService] Scene is not available in build settings: " + destinationScene);
            return;
        }

        if (!showLoading)
        {
            SceneManager.LoadScene(destinationScene);
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(StringManager.SCENE_LOADING))
        {
            Debug.LogError("[SceneFlowService] Loading scene is not available in build settings.");
            return;
        }

        LoadingScreen.SCENE = destinationScene;
        SceneManager.LoadScene(StringManager.SCENE_LOADING);
    }
}
