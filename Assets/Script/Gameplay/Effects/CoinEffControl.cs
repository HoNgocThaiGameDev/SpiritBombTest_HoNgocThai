using UnityEngine;
using System.Collections;

public class CoinEffControl : MonoBehaviour
{
    public int size = 0;
    Rigidbody2D rBody;
    private bool isTarget;
    private float distance;
    private Quaternion newRotation;
    Vector3 scale;
    private Vector3 bigSize = new Vector3(0.7f, 0.7f, 1f);
    private Vector3 normalSize = new Vector3(0.5f, 0.5f, 1f);
    private Vector3 smallSize = new Vector3(0.35f, 0.35f, 1f);
    // Use this for initialization
    void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }
    Vector3 vel = Vector3.one;
    void OnEnable()
    {
        if (GameState.currentLevel != 1)
            rBody.AddForce(Random.insideUnitCircle, ForceMode2D.Force);
    }
    void Update()
    {
        if (GameState.isGamePaused == false)
        {
            if (isTarget)
            {
                newRotation = Quaternion.LookRotation(transform.position - MyPlaneController.instance.transform.position, Vector3.forward);
                newRotation.x = 0.0f;
                newRotation.y = 0.0f;
                transform.rotation = newRotation;//Quaternion.Slerp(transform.rotation, newRotation, 0.15f);
                rBody.velocity = transform.up * 8;
                //transform.position = Vector3.SmoothDamp(transform.position, MyPlaneController.instance.transform.position, ref vel, 0.2f);
            }
            else
            {
                distance = Vector3.Distance(MyPlaneController.instance.transform.position, transform.position);
                {
                    if (distance <= 2f)
                    {
                        isTarget = true;
                    }
                }
            }
        }
        if (GameState.currentLevel == 1 && transform.position.y < -5f)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnDisable()
    {
        isTarget = false;
        if (GameManager.Instance != null)
            GameManager.Instance.goldPool.Store(this);
    }
    public void SetInfo(Vector3 newPos)
    {
        //transform.position = newPos;
        // tỷ lê 40:40:20
        int random = Random.Range(0, 101);
        if (random < 40)
        {
            transform.localScale = smallSize;
            size = 1;
        }
        else if (random >= 40 && random <= 80)
        {
            transform.localScale = normalSize;
            size = 2;
        }
        else
        {
            transform.localScale = bigSize;
            size = 3;
        }
        transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        transform.position = newPos + Random.insideUnitSphere / 2;
        //transform.position = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        gameObject.SetActive(true);
    }
}
