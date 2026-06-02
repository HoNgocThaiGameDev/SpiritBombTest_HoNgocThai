using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CoinControl : MonoBehaviour
{
    Vector3 distance;
    bool inside;
    void FixedUpdate()
    {
        if (inside)
        {
            transform.DOMove(GameObject.Find("Player").transform.position, 2);
            inside = false;
        }
    }
}
