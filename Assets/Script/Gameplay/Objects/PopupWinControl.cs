using UnityEngine;
using System.Collections;

public class PopupWinControl : MonoBehaviour
{
    private Animator animator;

    public GameObject[] star;
    public GameObject[] fxStar;

    private WaitForSeconds wait1;
    private WaitForSeconds wait04;
    private WaitForSeconds wait2;

    void Start()
    {
        wait1 = new WaitForSeconds(1);
        wait2 = new WaitForSeconds(2);
        wait04 = new WaitForSeconds(0.4f);
        animator = GetComponent<Animator>();

        if (SoundController.instance != null)
            SoundController.instance.StopAllSound();

        if (GameState.currentLevel != 1)
        {
            StartCoroutine(ActiveStar(GameState.countSaveMe));
        }
        else
        {
            StartCoroutine(ActiveStar(0));
        }
    }
    IEnumerator ActiveStar(int index)
    {
        if (index >= 2)
        {
            yield return wait1;
            ShowStar(0);
            GameState.SaveLevelComplete(1);
        }
        else if (index == 1)
        {
            yield return wait1;
            ShowStar(0);
            yield return wait04;
            ShowStar(1);
            GameState.SaveLevelComplete(2);
        }
        else
        {
            yield return wait1;
            ShowStar(0);
            yield return wait04;
            ShowStar(1);
            yield return wait04;
            ShowStar(2);
            GameState.SaveLevelComplete(3);
        }
        yield return wait2;
        if (animator != null)
            animator.Play("PopupWinClose");

        int starCount = star != null ? Mathf.Min(3, star.Length) : 0;
        for (int i = 0; i < starCount; i++)
        {
            if (star[i] != null && star[i].activeInHierarchy)
                GameState.countStar.Value++;
        }
    }

    private void ShowStar(int index)
    {
        SetActive(star, index, true);
        SetActive(fxStar, index, true);
    }

    private static void SetActive(GameObject[] targets, int index, bool active)
    {
        if (targets == null || index < 0 || index >= targets.Length || targets[index] == null)
            return;

        targets[index].SetActive(active);
    }
}
