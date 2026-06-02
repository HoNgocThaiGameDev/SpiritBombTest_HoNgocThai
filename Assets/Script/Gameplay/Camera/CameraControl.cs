using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;

    public GameObject missionComplete;    // ban xong mot thang boss
    public GameObject missionLose;    // ban xong mot thang boss
    public GameObject myPlain;            // may bay chinhu
    public GameObject winResult;
    public GameObject loseResult;

    public bool isStop;
    bool activeWon;
    private WaitForSeconds waitShowBannerWin1;
    private WaitForSeconds waitShowBannerWin2;
    private WaitForSeconds waitShowBannerLose;
    void Awake()
    {
        instance = this;
        EnsureSingleAudioListener();
        waitShowBannerWin1 = new WaitForSeconds(4);
        waitShowBannerWin2 = new WaitForSeconds(1.5f);
        waitShowBannerLose = new WaitForSeconds(3);
    }
    void Start()
    {
        SetActiveIfPresent(missionComplete, false);
        SetActiveIfPresent(missionLose, false);
        SetActiveIfPresent(winResult, false);
        SetActiveIfPresent(loseResult, false);
        SetActiveIfPresent(myPlain, true);
        EnsureSingleAudioListener();
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameState.isGamePaused = false;
        GameState.isMoveMap = true;
        GameState.won = false;
        GameState.lose = false;
    }

    private void SetActiveIfPresent(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }

    private void EnsureSingleAudioListener()
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length == 0)
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                camera.gameObject.AddComponent<AudioListener>();
            }
            return;
        }

        bool keptOne = false;
        for (int i = 0; i < listeners.Length; i++)
        {
            if (!keptOne && listeners[i].gameObject.activeInHierarchy)
            {
                listeners[i].enabled = true;
                keptOne = true;
            }
            else
            {
                listeners[i].enabled = false;
            }
        }
    }

    void Update()
    {
        if (GameState.won == true)
        {
            if (!activeWon)
            {
                StartCoroutine(ShowBannerWin());
                activeWon = true;
            }
        }
        if (GameState.lose == true)
        {
            if (!activeWon)
            {
                StartCoroutine(ShowBannerLose());
                activeWon = true;
            }
        }
    }
    IEnumerator ShowBannerWin()
    {
        missionComplete.SetActive(true);
        yield return waitShowBannerWin1;
        MyPlaneController.instance.isMoveEnd = true;
        yield return waitShowBannerWin2;
        MyPlaneController.instance.gameObject.SetActive(false);
        winResult.SetActive(true);
    }
    IEnumerator ShowBannerLose()
    {
        missionLose.SetActive(true);
        yield return waitShowBannerLose;
        loseResult.SetActive(true);
    }

    
}
