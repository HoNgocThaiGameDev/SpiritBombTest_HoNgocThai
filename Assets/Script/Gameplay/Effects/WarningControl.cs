using UnityEngine;
using System.Collections;

public class WarningControl : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.Play(0, 0, 0f);

        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
}
