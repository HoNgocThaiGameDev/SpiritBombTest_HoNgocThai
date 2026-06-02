using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class ExplosionControl : MonoBehaviour
{
    public ExplosionType type;
    private WaitForSeconds waitToDie;
    private Vector3 originalPos ;
    void Awake()
    {
        waitToDie = new WaitForSeconds(2f);
    }
    // Use this for initialization
    void OnEnable()
    {
        StartCoroutine(SelfDestroy());
        originalPos = transform.position;
    }

    IEnumerator SelfDestroy()
    {
        yield return waitToDie;
        gameObject.SetActive(false);
    }

    public void SetInfo(Vector3 newPos)
    {
        transform.position = newPos;
        gameObject.SetActive(true);
    }
    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            switch (type)
            {
                case ExplosionType.ExplosionE01:
                    GameManager.Instance.explosionE01Pool.Store(this);
                    break;
                case ExplosionType.ExplosionFix1:
                    GameManager.Instance.explosionFix1Pool.Store(this);
                    break;
                case ExplosionType.ExplosionFix2:
                    GameManager.Instance.explosionFix2Pool.Store(this);
                    break;
                case ExplosionType.ExplosionBoss:
                    GameManager.Instance.explosionBossPool.Store(this);
                    break;
            }
        }
    }
}

public enum ExplosionType
{
    ExplosionE01,
    ExplosionFix1,
    ExplosionFix2,
    ExplosionBoss,
    ExplosionAll,
    ExplosionUpgrade
}
