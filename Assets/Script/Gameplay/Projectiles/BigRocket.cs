using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRocket : MonoBehaviour
{

    public float speed;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        if (transform.position.y < -5f)
        {
            gameObject.SetActive(false);
        }
    }

  
}
