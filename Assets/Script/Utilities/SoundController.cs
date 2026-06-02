using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    public GameObject[] listSound;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        AudioListener.pause = !PlayerSettingsService.IsSoundEnabled();
        PlaySound(8);
    }

    public void PlaySound(int index)
    {
        if (AudioListener.pause && PlayerSettingsService.IsSoundEnabled())
            AudioListener.pause = false;
        listSound[index].SetActive(true);
    }

    public void StopSound(int index)
    {
        listSound[index].SetActive(false);
    }

    public void StopAllSound()
    {
        for (int i = 0; i < listSound.Length; i++)
        {
            listSound[i].SetActive(false);
        }
    }
}
