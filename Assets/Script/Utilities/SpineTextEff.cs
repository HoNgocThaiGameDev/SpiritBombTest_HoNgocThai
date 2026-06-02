using UnityEngine;
using System.Collections;

public class SpineTextEff : MonoBehaviour
{

    // Use this for initialization
    void OnEnable()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.4f);
        gameObject.SetActive(false);
    }
    public void SetInfo(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
