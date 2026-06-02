using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoadingScreen : MonoBehaviour
{
    public static string SCENE = "Menu";

    [SerializeField] private GameObject spineLoading;
    [FormerlySerializedAs("camera"), SerializeField] private GameObject loadingCamera;
    [SerializeField] private GameObject canvasLoad;
    [SerializeField] private TMP_Text progress;
    [SerializeField] private TMP_Text tip;
    [SerializeField, Min(0.1f)] private float progressSpeed = 90f;
    [SerializeField, Min(0f)] private float completedDelay = 0.05f;

    private Animator animator;
    private AsyncOperation loadOperation;
    private float displayedProgress;

    private IEnumerator Start()
    {
        SetupLoadingScreen();
        yield return LoadScene();
    }

    private void SetupLoadingScreen()
    {
        if (GameState.listTip != null && GameState.listTip.Length > 0 && tip != null)
            tip.text = GameState.listTip[Random.Range(0, GameState.listTip.Length)];

        if (spineLoading != null)
        {
            animator = spineLoading.GetComponent<Animator>();
            spineLoading.SetActive(true);
        }

        SetActiveIfPresent(canvasLoad, true);
        SetActiveIfPresent(loadingCamera, true);
        SetProgress(0f);
    }

    private IEnumerator LoadScene()
    {
        loadOperation = SceneManager.LoadSceneAsync(SCENE);
        if (loadOperation == null)
            yield break;

        loadOperation.allowSceneActivation = false;

        while (displayedProgress < 100f)
        {
            float loadProgress = Mathf.Clamp01(loadOperation.progress / 0.9f) * 100f;
            float targetProgress = loadOperation.progress >= 0.9f ? 100f : loadProgress;
            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, progressSpeed * Time.unscaledDeltaTime);
            SetProgress(displayedProgress);
            yield return null;
        }

        SetProgress(100f);
        if (animator != null)
            animator.SetBool("isLoadCompleted", true);

        if (completedDelay > 0f)
            yield return new WaitForSecondsRealtime(completedDelay);

        loadOperation.allowSceneActivation = true;
    }

    private void SetProgress(float value)
    {
        displayedProgress = Mathf.Clamp(value, 0f, 100f);
        if (progress != null)
            progress.text = "LOADING: " + Mathf.RoundToInt(displayedProgress).ToString() + "%";
    }

    private static void SetActiveIfPresent(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }
}
