using UnityEngine;
using System.Collections;

public class RotateObj : MonoBehaviour
{
    public float speed;
    public bool counterClock;

    void Update()
    {
        if (counterClock)
        {
            transform.Rotate(Vector3.forward * speed);
        }
        else
        {
            //transform.Rotate(Vector3.t * Time.deltaTime * speed);
            transform.Rotate(Vector3.forward * speed * -1);
        }
    }
}
