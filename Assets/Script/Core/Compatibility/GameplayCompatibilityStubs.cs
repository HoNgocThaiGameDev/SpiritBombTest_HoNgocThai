using UnityEngine;

public static class GameNotice
{
    public static void Show(string notice)
    {
        Debug.Log(notice);
    }
}

public static class GameplayTimers
{
    public static string name_player = "Player";
    public static int[] TIME_UPGRADE = { 0, 0, 180, 300, 300, 420, 420, 600 };
    public static int[] COST_CRYSTAL_SKIP = { 0, 0, 3, 5, 5, 8, 8, 10 };
    public static float timeCountNow;
    public static float timeCountUpgrade;
    public static bool processCountTime = true;
}

public class TutorialController : MonoBehaviour
{
    private static TutorialController _instance;

    public static TutorialController instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("TutorialController");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<TutorialController>();
            }

            return _instance;
        }
    }

    public bool isTutorialBox;
    public bool isTutorialLevel3;
    public bool isTutorialLevel4;

    public void CheckTutorial() { }
    public void CheckTutorialLevel3() { }
    public void CheckTutorialLevel4() { }
    public void CheckTutorialOpenBox() { }
    public void CheckTutorialFirst() { }
}

public class UITutorialController : MonoBehaviour
{
    private static UITutorialController _instance;

    public static UITutorialController instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("UITutorialController");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<UITutorialController>();
            }

            return _instance;
        }
    }

    public void GetObjectTutorial() { }
}
