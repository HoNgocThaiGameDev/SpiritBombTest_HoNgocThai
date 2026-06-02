using UnityEngine;
using System.Collections;

public class ItemCollisEffect : MonoBehaviour {

    private WaitForSeconds waitToDie;
    void Awake()
    {
        waitToDie = new WaitForSeconds(1f);
    }
    // Use this for initialization
    void OnEnable()
    {
        StartCoroutine(SelfDestroy());
    }

    IEnumerator SelfDestroy()
    {
        yield return waitToDie;
        gameObject.SetActive(false);
    }

    public void SetInfo(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
