using UnityEngine;
using System.Collections;

public class RocketControlItem : MonoBehaviour
{
    Collider2D target;
    Rigidbody2D body;
    float MissileSpeed;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        MissileSpeed = GameState.speedRocket;
    }
    void OnEnable()
    {
        FindTarget();
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(90,150)));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(270,330)));
        }
        StartCoroutine(WaitDestroy());
    }
    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        ExplosionControl explosion = GameManager.Instance.explosionFix1Pool.New();
        explosion.SetInfo(transform.position);
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
    void Update()
    {
        if (target == null)
        {
            Vector3 pseudoTarget = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
            Quaternion newRotation = Quaternion.LookRotation(transform.position - pseudoTarget, Vector3.forward);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.15f);
            body.velocity = transform.up * MissileSpeed;
        }
        else
        {
            Quaternion newRotation = Quaternion.LookRotation(transform.position - target.transform.position, Vector3.forward);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.15f);
            body.velocity = transform.up * MissileSpeed;
        }
    }
    void FindTarget()
    {
        Vector3 radaPos = new Vector3(transform.position.x + 0.5f, transform.position.y + 1.5f, transform.position.z);
        target = Physics2D.OverlapCircle(radaPos, 5f, 1 << LayerMask.NameToLayer("GamePlay"));

    }
}
