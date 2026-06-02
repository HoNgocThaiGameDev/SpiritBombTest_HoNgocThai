using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class UIFadePulse : MonoBehaviour
{
    [SerializeField] private Graphic targetGraphic;
    [SerializeField, Range(0f, 1f)] private float minAlpha = 0.25f;
    [SerializeField, Range(0f, 1f)] private float maxAlpha = 1f;
    [SerializeField, Min(0.05f)] private float fadeDuration = 0.45f;
    [SerializeField, Min(0f)] private float intervalDuration = 0.15f;
    [SerializeField] private bool useUnscaledTime = true;

    private Coroutine fadeCoroutine;

    private void Reset()
    {
        targetGraphic = GetComponent<Graphic>();
    }

    private void Awake()
    {
        if (targetGraphic == null)
            targetGraphic = GetComponent<Graphic>();
    }

    private void OnEnable()
    {
        if (targetGraphic == null)
            return;

        SetAlpha(maxAlpha);
        fadeCoroutine = StartCoroutine(FadeLoop());
    }

    private void OnDisable()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        SetAlpha(maxAlpha);
    }

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return Fade(maxAlpha, minAlpha);
            yield return WaitInterval();
            yield return Fade(minAlpha, maxAlpha);
            yield return WaitInterval();
        }
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += GetDeltaTime();
            SetAlpha(Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / fadeDuration)));
            yield return null;
        }

        SetAlpha(to);
    }

    private IEnumerator WaitInterval()
    {
        float elapsed = 0f;
        while (elapsed < intervalDuration)
        {
            elapsed += GetDeltaTime();
            yield return null;
        }
    }

    private float GetDeltaTime()
    {
        return useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
    }

    private void SetAlpha(float alpha)
    {
        if (targetGraphic == null)
            return;

        Color color = targetGraphic.color;
        color.a = alpha;
        targetGraphic.color = color;
    }
}
