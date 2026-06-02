using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public static EnemyProjectile instance;
    public int damage;
    public float speed = 4f;

    private Rigidbody2D rBody;
    private bool launched;

    private void Awake()
    {
        instance = this;
        rBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        instance = this;
        if (rBody == null)
        {
            rBody = GetComponent<Rigidbody2D>();
        }

        if (!launched)
        {
            AimAtPlayer();
        }
    }

    private void OnDisable()
    {
        launched = false;
    }

    public void Launch(Vector2 velocity, int projectileDamage)
    {
        damage = projectileDamage;
        launched = true;

        if (rBody == null)
        {
            rBody = GetComponent<Rigidbody2D>();
        }

        if (rBody != null)
        {
            rBody.velocity = velocity;
        }

        if (velocity.sqrMagnitude > 0.001f)
        {
            transform.up = velocity.normalized;
        }
    }

    private void AimAtPlayer()
    {
        MyPlaneController player = MyPlaneController.instance;
        Vector3 velocity = player != null && player.transform != null
            ? (player.transform.position - transform.position).normalized * speed
            : Vector3.down * speed;

        if (rBody != null)
        {
            rBody.velocity = velocity;
        }
    }

    private void Update()
    {
        if (GameManager.isDestroyAll)
        {
            gameObject.SetActive(false);
        }

        if (WaveManager.instance != null && WaveManager.instance.isShowWin)
        {
            gameObject.SetActive(false);
        }
    }
}
