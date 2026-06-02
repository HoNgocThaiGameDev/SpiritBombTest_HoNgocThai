using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIDialog : MonoBehaviour
{
    public static UIDialog instance;

    public GameObject target;
    public GameObject bgDialog;
    public TMP_Text txtDialog;

    private Vector3 dialogStartPosition;
    private IEnumerator disappearRoutine;

    private void Start()
    {
        if (instance == null)
            instance = this;

        dialogStartPosition = bgDialog.transform.position;
    }

    public void ShowDialog(string notice)
    {
        bgDialog.transform.DOKill();
        if (disappearRoutine != null)
            StopCoroutine(disappearRoutine);

        bgDialog.transform.position = dialogStartPosition;
        bgDialog.SetActive(true);
        txtDialog.text = notice;
        bgDialog.transform.DOMove(target.transform.position, 1f);

        disappearRoutine = DisappearText();
        StartCoroutine(disappearRoutine);
    }

    private IEnumerator DisappearText()
    {
        yield return new WaitForSeconds(3f);
        bgDialog.SetActive(false);
        bgDialog.transform.position = dialogStartPosition;
    }
}
