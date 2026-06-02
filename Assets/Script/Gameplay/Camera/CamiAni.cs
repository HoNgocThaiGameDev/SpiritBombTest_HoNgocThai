using UnityEngine;
using System.Collections;

/// <summary>
/// rung camera
/// </summary>
public class CamiAni : MonoBehaviour
{

    public static CamiAni instance;

    public bool shakeCam = false;
    public Animation _ani;
    private WaitForSeconds timeWait;
    void Start()
    {
        instance = this;
        timeWait = new WaitForSeconds(0.4f);
    }

    public void StartShake()
    {
        if (!_ani.enabled)
        {
            StartCoroutine(WaitStopShake());
        }
    }

    IEnumerator WaitStopShake()
    {
        _ani.enabled = true;
        yield return timeWait;
        StopShake();
    }

    public void StopShake()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        _ani.enabled = false;
    }
}
