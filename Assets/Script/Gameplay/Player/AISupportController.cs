using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISupportController : MonoBehaviour
{
    public static AISupportController instance;
    public Transform fireBullet;
    private float nextFire = 0;
    private float fireRate = 0.4f;
    public bool isAttack = false;
    public GameObject skillDestroy;
    private WaitForSeconds timeWait;
    private WaitForSeconds timeWaitSkill;
    private WaitForSeconds timeWaitSkill2;
    private bool isActiveSkill = false;
    private void Awake()
    {
        instance = this;
        timeWait = new WaitForSeconds(5f);
        timeWaitSkill = new WaitForSeconds(1.5f);
        timeWaitSkill2 = new WaitForSeconds(3f);
    }

    // Use this for initialization
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2.5f);
        isAttack = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameState.won)
        {
            isAttack = false;
        }
        if (isAttack)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                FireDarkBuster();
            }
        }
        if (WaveManager.instance.numberWave == 6 || WaveManager.instance.numberWave == 8 )
        {
            if (!isActiveSkill)
            {
                StartCoroutine(WaitHideDestroy());
                isActiveSkill = true;
            }
        }
    }

    IEnumerator WaitHideDestroy()
    {
        yield return timeWaitSkill;
        skillDestroy.SetActive(true);
        isAttack = false;
        yield return timeWait;
        isAttack = true;
        skillDestroy.SetActive(false);
        yield return timeWaitSkill2;
        isActiveSkill = false;
    }

private void FireDarkBuster()
    {
        ObjectPooler supportPool = ObjectPoolerManager.Instance != null ? ObjectPoolerManager.Instance.bulletSP1 : null;
        if (supportPool == null || fireBullet == null)
        {
            return;
        }

        GameObject bullet = supportPool.GetPooledObject();
        bullet.transform.position = fireBullet.position;
        bullet.transform.localEulerAngles = fireBullet.localEulerAngles;
        bullet.SetActive(true);
    }
}
