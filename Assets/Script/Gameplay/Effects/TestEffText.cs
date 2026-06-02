using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TestEffText : MonoBehaviour
{
    public SpriteRenderer spriteRender;

    private WaitForSeconds waitToDie;
    private readonly Color originalColor = Color.white;

    private void Awake()
    {
        waitToDie = new WaitForSeconds(0.8f);
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one / 10f;
        spriteRender.color = originalColor;
        transform.DOScale(Vector3.one, 0.5f);
        spriteRender.DOFade(0f, 2f).SetEase(Ease.InOutElastic);
        StartCoroutine(SelfDestroy());
    }

    private void OnDisable()
    {
        transform.DOKill();
        if (spriteRender != null)
            spriteRender.DOKill();

        if (GameManager.Instance != null)
            GameManager.Instance.NotiPool.Store(this);
    }

    private IEnumerator SelfDestroy()
    {
        yield return waitToDie;
        gameObject.SetActive(false);
    }

    public void SetInfo(Sprite newSprite, Vector3 newPosition)
    {
        transform.position = newPosition;
        spriteRender.sprite = newSprite;
    }
}
