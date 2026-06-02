using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRocketController : MonoBehaviour
{
    public GameObject target;
    private WaitForSeconds timeWaitMove;

    void Awake()
    {
        timeWaitMove = new WaitForSeconds(2.5f);
    }

    private void OnEnable()
    {
        if (WaveManager.instance != null)
        {
            WaveManager.instance.countEnemy += 1;
        }
        StartCoroutine(WaitSpawnRocket());
    }

    IEnumerator WaitSpawnRocket()
    {
        yield return timeWaitMove;
        BigRocket rocket = GameManager.Instance.bigRocketPool.New();
        rocket.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2f, 0);
        rocket.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (WaveManager.instance != null)
        {
            WaveManager.instance.countEnemy -= 1;
        }
    }

}
