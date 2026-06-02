using UnityEngine;
using System.Collections;

public class Boundary2Controll : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("danta"))
        {
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("dandich") || other.CompareTag("gold") || other.CompareTag("exp") || other.CompareTag("dandich2"))
        {
            other.gameObject.SetActive(false);
        }

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("gold"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
