using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    public int typeCore;
    private Rigidbody2D rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        rBody.AddForce(Random.insideUnitCircle, ForceMode2D.Force);
    }

    public void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            switch (typeCore)
            {
                case 1:
                    GameManager.Instance.corePool1.Store(this);
                    break;
                case 2:
                    GameManager.Instance.corePool2.Store(this);
                    break;
                case 3:
                    GameManager.Instance.corePool3.Store(this);
                    break;
            }
        }

    }

    public void SetInfo(Vector3 newPos)
    {
        transform.position = newPos + Random.insideUnitSphere / 2;
        gameObject.SetActive(true);
    }

}
