using UnityEngine;
using System.Collections;

public class Itemcontroller : MonoBehaviour
{
    public Rigidbody2D rBody;
    public SpriteRenderer spriteRender;
    private WaitForSeconds waitToSelfDestroy;

    void Awake()
    {
        waitToSelfDestroy = new WaitForSeconds(6f);
    }
    void OnEnable()
    {
        if (GameState.currentLevel != 1)
        {
            StartCoroutine(AutoDestroy());
            rBody.AddForce(Random.insideUnitCircle, ForceMode2D.Force);
        }
        else
        {
            tag = GameManager.Instance.listItemType[0].tag; // tag speed att
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.itemPool.Store(this);
    }

    IEnumerator AutoDestroy()
    {
        yield return waitToSelfDestroy;
        gameObject.SetActive(false);
    }
    public void SetInfo(int index, Vector3 newPosition)
    {
        transform.position = newPosition;
        tag = GameManager.Instance.listItemType[index].tag;
        spriteRender.sprite = GameManager.Instance.listItemType[index].sprite;
        //tag = tagStr;
    }
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    // va cham vs main
    //    if (other.CompareTag("Player"))
    //    {
    //        TestEffText Noti = GameManager.Instance.NotiPool.New();
    //        Noti.SetInfo(GameManager.Instance.NotiTextList[5],other.transform.position);
    //    }
    //}
}
