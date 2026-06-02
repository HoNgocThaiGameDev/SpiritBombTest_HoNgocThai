using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ShowScoreText : MonoBehaviour
{
    [HideInInspector]
    public int _score;
    public TextMesh scoreText;

    private WaitForSeconds waitToDie;
    private Vector3 startPosition;

    private void Awake()
    {
        waitToDie = new WaitForSeconds(0.8f);
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        scoreText.text = _score.ToString();
        transform.position = startPosition;
        transform.DOMoveY(transform.position.y + 0.5f, 1f);
        StartCoroutine(SelfDestroy());
    }

    private void OnDisable()
    {
        transform.DOKill();
        if (GameManager.Instance != null)
            GameManager.Instance.scoreAddedPool.Store(this);
    }

    private IEnumerator SelfDestroy()
    {
        yield return waitToDie;
        transform.DOKill();
        gameObject.SetActive(false);
    }

    public void SetInfo(int newScore, Vector3 newPosition)
    {
        _score = newScore;
        startPosition = newPosition;
        gameObject.SetActive(true);
    }
}
