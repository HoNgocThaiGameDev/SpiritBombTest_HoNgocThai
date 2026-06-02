using UnityEngine;
using System.Collections;

public class PopupLoseControl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (MyPlaneController.instance != null && MyPlaneController.instance.colliderPlayer != null)
        {
            MyPlaneController.instance.colliderPlayer.enabled = false;
        }
        if (SoundController.instance != null)
        {
            SoundController.instance.StopAllSound();
        }
    }

}
