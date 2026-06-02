using UnityEngine;
using System.Collections;

public class BoundaryControll : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ListTag.TAG_DANTA) || other.CompareTag(ListTag.TAG_DAN5) || other.CompareTag(ListTag.TAG_DANTA2))
        {
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag(ListTag.TAG_DANDICH) || other.CompareTag(ListTag.TAG_DANDICH2))
        {
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag(ListTag.TAG_ITEMGOLD))
        {
            if (GameState.currentLevel != 1)
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
