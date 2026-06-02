using UnityEngine;
using System.Collections;

public class UILoading : MonoBehaviour
{
    public static UILoading instance;
    IEnumerator _waitClose;

    void start() {
        instance = this;
        _waitClose = WaitClose();
    }

    void OnEnable()
    {
        _waitClose = WaitClose();
        StartCoroutine(_waitClose);
    }
    IEnumerator WaitClose()
    {
        yield return new WaitForSeconds(5);
        if (gameObject.activeInHierarchy && gameObject != null)
            gameObject.SetActive(false);
    }

    public void StopCoroutine() {
        StopCoroutine(_waitClose);
    }
}
