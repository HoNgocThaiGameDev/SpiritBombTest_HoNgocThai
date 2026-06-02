using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [Tooltip("Enable to unlock every level. Disable to use the player's completed-level progress.")]
    public bool unlockAllLevels;
    public GameObject[] lockLevel;
    void Awake()
    {
        instance = this;
        GameState.LevelFull = unlockAllLevels;
        LevelItem[] levelItems = FindObjectsOfType<LevelItem>(true);
        if (levelItems.Length > 0)
        {
            Array.Sort(levelItems, (left, right) => left.level.CompareTo(right.level));
            for (int i = 0; i < levelItems.Length; i++)
            {
                SetLockActive(levelItems[i].lockItem, levelItems[i].level > GameState.GetLevelCompleted() + 1);
            }
            return;
        }

        for (int i = 0; lockLevel != null && i < lockLevel.Length; i++)
        {
            SetLockActive(lockLevel[i], i > GameState.GetLevelCompleted());
        }
    }

    private void SetLockActive(GameObject lockObject, bool lockedByProgress)
    {
        if (lockObject != null)
        {
            lockObject.SetActive(!unlockAllLevels && lockedByProgress);
        }
    }
}
