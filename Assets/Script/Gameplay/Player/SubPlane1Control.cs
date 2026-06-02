using UnityEngine;
using System.Collections;

public class SubPlane1Control : MonoBehaviour
{
    public static SubPlane1Control instance;

    private float nextFire = 0;

    public int idBulletSP;

    public Transform[] posAtt;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (idBulletSP != 1 || GameState.won || Time.time <= nextFire)
        {
            return;
        }

        nextFire = Time.time + MyPlaneController.instance.subFireRate;
        ObjectPooler supportPool = ObjectPoolerManager.Instance != null ? ObjectPoolerManager.Instance.bulletSP1 : null;
        if (supportPool == null || posAtt == null || posAtt.Length == 0)
        {
            return;
        }

        SpawnSupportBullet(supportPool, posAtt[0]);

        if (MyPlaneController.instance.level > 4 && posAtt.Length > 2)
        {
            SpawnSupportBullet(supportPool, posAtt[1]);
            SpawnSupportBullet(supportPool, posAtt[2]);
        }
    }

private void SpawnSupportBullet(ObjectPooler supportPool, Transform firePoint)
    {
        if (supportPool == null || firePoint == null)
        {
            return;
        }

        GameObject bullet = supportPool.GetPooledObject();
        bullet.transform.position = firePoint.position;
        bullet.transform.localEulerAngles = firePoint.localEulerAngles;
        bullet.SetActive(true);
    }


private void FirePhoenix()
    {
    }
private void FireDarkBuster()
    {
    }

}
