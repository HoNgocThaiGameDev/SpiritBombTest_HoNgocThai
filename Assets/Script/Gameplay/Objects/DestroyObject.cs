using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {

	// Use this for initialization
    public float timeDes;
	void Start () {
        StartCoroutine(WaitHide());
	}
    void OnEnable()
    {
        StartCoroutine(WaitHide());
    }
    IEnumerator WaitHide()
    {
        yield return new WaitForSeconds(timeDes);
        gameObject.SetActive(false);
    }
	
}
