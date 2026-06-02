using UnityEngine;
using System.Collections;

public class StarCollect : MonoBehaviour
{
    public GameObject cameraObject;
    Camera cameraX;
    void Awake()
    {
        cameraX = cameraObject.GetComponent<Camera>();
    }
    void OnEnable()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        cameraX.orthographicSize = 3.9f;
        yield return new WaitForSeconds(0.25f);
        cameraX.orthographicSize = 4;
    }
}
