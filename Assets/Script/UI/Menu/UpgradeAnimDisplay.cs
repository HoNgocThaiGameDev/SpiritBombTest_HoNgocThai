using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAnimDisplay : MonoBehaviour
{


    [SerializeField]
    private Animator mainAnimation;

    [SerializeField]
    private Animator support1Animation;

    [SerializeField]
    private Animator support2Animation;
    // Update is called once per frame

    public void Upgarde()
    {
        mainAnimation.SetTrigger("upgrade");
        support1Animation.SetTrigger("upgrade");
        support2Animation.SetTrigger("upgrade");
    }

    public void SetAnim()
    {
        if (UIUpgradeControl.instance.currentLevel <= 2)
        {
            mainAnimation.Play("1-1");
            support1Animation.Play("1-1");
            support2Animation.Play("1-1");
        }
        else if (UIUpgradeControl.instance.currentLevel <= 4)
        {
            mainAnimation.Play("2-2");
            support1Animation.Play("2-2");
            support2Animation.Play("2-2");
        }
        else if (UIUpgradeControl.instance.currentLevel <= 6)
        {
            mainAnimation.Play("3-3");
            support1Animation.Play("3-3");
            support2Animation.Play("3-3");
        }
        else if (UIUpgradeControl.instance.currentLevel <= 8)
        {
            mainAnimation.Play("4-4");
            support1Animation.Play("4-4");
            support2Animation.Play("4-4");
        }                              
    }

    public void ShowUpgradeAnim()
    {
        if (UIUpgradeControl.instance.currentLevel == 3 ||
            UIUpgradeControl.instance.currentLevel == 5 ||
            UIUpgradeControl.instance.currentLevel == 7)
        {
            mainAnimation.SetBool("isReturn", false);
            support1Animation.SetBool("isReturn", false);
            support2Animation.SetBool("isReturn", false);
        }
        else
        {
            mainAnimation.SetBool("isReturn", true);
            support1Animation.SetBool("isReturn", true);
            support2Animation.SetBool("isReturn", true);
        }

        mainAnimation.SetTrigger("upgrade");
        support1Animation.SetTrigger("upgrade");
        support2Animation.SetTrigger("upgrade");

    }
}
