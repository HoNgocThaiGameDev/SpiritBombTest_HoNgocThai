using System.Collections;
using UnityEngine;

public class BulletPatternController : MonoBehaviour
{
    private static BulletPatternController instance;

    private Coroutine bossPatternRoutine;
    private Transform bossOrigin;
    private int bossDamage;

    public static BulletPatternController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BulletPatternController>();
            }

            if (instance == null)
            {
                GameObject controller = new GameObject(nameof(BulletPatternController));
                instance = controller.AddComponent<BulletPatternController>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void StartBossPattern(Transform origin, int damage)
    {
        bossOrigin = origin;
        bossDamage = damage;

        if (bossPatternRoutine != null)
        {
            StopCoroutine(bossPatternRoutine);
        }

        bossPatternRoutine = StartCoroutine(BossPattern());
    }

    public void StopBossPattern()
    {
        if (bossPatternRoutine != null)
        {
            StopCoroutine(bossPatternRoutine);
            bossPatternRoutine = null;
        }
    }

    public void StopAllPatterns()
    {
        StopBossPattern();
    }

    public void FireEnemyDeathBurst(Vector3 position, int damage)
    {
        const int bulletCount = 12;
        const float speed = 3.1f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = 360f * i / bulletCount;
            FireEnemyProjectile(position, AngleToVelocity(angle, speed), damage);
        }
    }

    private IEnumerator BossPattern()
    {
        float angleOffset = 0f;

        while (bossOrigin != null && bossOrigin.gameObject.activeInHierarchy)
        {
            FireSpread(bossOrigin.position, 7, 75f, -90f, 3.6f, bossDamage);
            yield return new WaitForSeconds(0.55f);

            FireRing(bossOrigin.position, 14, angleOffset, 2.8f, bossDamage);
            angleOffset += 15f;
            yield return new WaitForSeconds(0.9f);
        }

        bossPatternRoutine = null;
    }

    private void FireSpread(Vector3 position, int count, float arc, float centerAngle, float speed, int damage)
    {
        if (count <= 1)
        {
            FireEnemyProjectile(position, AngleToVelocity(centerAngle, speed), damage);
            return;
        }

        float startAngle = centerAngle - arc * 0.5f;
        float step = arc / (count - 1);
        for (int i = 0; i < count; i++)
        {
            FireEnemyProjectile(position, AngleToVelocity(startAngle + step * i, speed), damage);
        }
    }

    private void FireRing(Vector3 position, int count, float angleOffset, float speed, int damage)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = angleOffset + 360f * i / count;
            FireEnemyProjectile(position, AngleToVelocity(angle, speed), damage);
        }
    }

    private void FireEnemyProjectile(Vector3 position, Vector2 velocity, int damage)
    {
        ObjectPoolerManager poolerManager = ObjectPoolerManager.Instance;
        if (poolerManager == null || poolerManager.DanDichPooler == null)
        {
            return;
        }

        GameObject projectileObject = poolerManager.DanDichPooler.GetPooledObject();
        if (projectileObject == null)
        {
            return;
        }

        projectileObject.transform.position = position;
        projectileObject.transform.rotation = Quaternion.identity;
        projectileObject.SetActive(true);

        EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();
        if (projectile != null)
        {
            projectile.Launch(velocity, damage);
            return;
        }

        Rigidbody2D body = projectileObject.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            body.velocity = velocity;
        }
    }

    private static Vector2 AngleToVelocity(float angle, float speed)
    {
        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * speed;
    }
}
