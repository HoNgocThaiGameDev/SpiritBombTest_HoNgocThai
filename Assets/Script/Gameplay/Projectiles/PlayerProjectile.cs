using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (MyPlaneController.instance != null)
        {
            speed = MyPlaneController.instance.speedAttack;
        }

        if (rBody != null)
        {
            rBody.velocity = transform.up * speed;
        }
    }
}
